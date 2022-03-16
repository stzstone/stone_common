using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using STZ_Common;
using StzEnums;

public class SundaytozResponseHandler : MonoBehaviour
{
	public delegate void ResponseDelegate(string jsonToken);

    //----------------------------------------------------------------
    // singleton
    //----------------------------------------------------------------
	private static SundaytozResponseHandler _instance;
    public static SundaytozResponseHandler Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            if (FindObjectOfType(typeof(SundaytozResponseHandler)) != null)
            {
                Debug.LogError("이미 SundaytozResponseHandler 가 존재합니다. 씬에서 제거해주세요. 앱을 종료합니다.");
                Application.Quit();
            }
                
            
            var container = new GameObject("SundaytozResponseHandler");
            _instance = container.AddComponent(typeof(SundaytozResponseHandler)) as SundaytozResponseHandler;
            DontDestroyOnLoad(container);

            return _instance;
        }
    }

    //----------------------------------------------------------------
    // ResponseHandle
    //----------------------------------------------------------------
    private interface IResponseHandle
    {
        void Parse(string jsonToken);
        void Invoke();
        void Clear();
    }

    private class ResponseHandle<T> : IResponseHandle where T : StzNativeResult_Base, new()
    {
        public T                    Result = null;
        public System.Action<T>     OnResponseCallback = null;

        public void Parse(string jsonToken)
        {
            Result = JsonUtility.FromJson<T>(jsonToken);
            if (Result.HasError)
                Debug.LogError($"[SundaytozResponseHandler.ResponseHandle.Parse] Has Error. code: {Result.ErrorCode},  msg: {Result.ErrorMsg ?? "-"}");
        }

        public void Invoke()
        {
            OnResponseCallback?.Invoke(Result);
        }

        public void Clear()
        {
            Result = null;
            OnResponseCallback = null;
        }
    }
	
    //----------------------------------------------------------------
    // variables
    //----------------------------------------------------------------
    private Dictionary<string, ResponseDelegate> _listenerSet = new Dictionary<string, ResponseDelegate>(); 

    private SundaytozPluginBase   _plugin          = null;
    private IResponseHandle       _responseHandle  = null;

    //----------------------------------------------------------------
    // get, set
    //----------------------------------------------------------------
    public StzNativeResult_Base  LastError { get; private set; } = new StzNativeResult_Base();
    
    //----------------------------------------------------------------
    // functions
    //----------------------------------------------------------------
    public bool InitPlugin(EPlatformType platform)
    {
        if (null != _plugin)
            return true;

        switch (platform)
        {
#if UNITY_IOS
        case EPlatformType.iOS:         _plugin = ScriptableObject.CreateInstance<SundaytozPluginiOS>();             break;
#endif
        case EPlatformType.Android:     _plugin = ScriptableObject.CreateInstance<SundaytozPluginAndroid>();         break;
        case EPlatformType.UnityEditor: _plugin = ScriptableObject.CreateInstance<SundaytozPluginForUnityEditor>();  break;

        default:
            Debug.LogError($"[SundaytozResponseHandler.InitPlugin] Invalid platform. platform : {platform}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 현재 네트워크 상태를 NOT_CONNECTED, WIFI, MOBILE로 구분해 반환 
    /// </summary>
    /// <returns></returns>
    public string GetNetworkStatus()
    {
        Debug.Log($"SundaytozResponseHandler::GetNetworkStatus(1)->_plugins is {(_plugin == null ? "null" : "not null")}");
        return _plugin == null ? ENetworkType.NOT_CONNECTED : _plugin.GetNetworkStatus();
    }

    public void SendRequest(IStzNativeCall call, System.Action<bool> doneCallback, bool inMultipleSendingEnable = false)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "ResponseHandler", "SendRequest", $"Begin. action: {(call?.GetAction() ?? "null")}");

        if (null == _plugin)
        {
            Debug.LogError("[SundaytozResponseHandler.SendRequest] SundaytozResponseHandler is not initialized.");
            doneCallback?.Invoke(false);
            return;
        }

        if (!inMultipleSendingEnable && null != _responseHandle)
        {
            Debug.LogError("[SundaytozResponseHandler.SendRequest] Already request action. Please try after responsed.");
            doneCallback?.Invoke(false);
            return;
        }

        _plugin.request(call);

        StzPluginLogger.Verbose("StzNativeExtension", "ResponseHandler", "SendRequest", $"End. action: {(call?.GetAction() ?? "null")}, result: TRUE");
        doneCallback?.Invoke(true);
    }

    public void SendRequest<T>(IStzNativeCall call, System.Action<T> onResponseCallback) where T : StzNativeResult_Base, new()
    {
        StzPluginLogger.Verbose("StzNativeExtension", "ResponseHandler", $"SendRequest<{(typeof(T)?.Name ?? "UNKNOWN")}>", $"Begin. action: {(call?.GetAction() ?? "null")}");

        if (null == _plugin)
        {
            Debug.LogError("[SundaytozResponseHandler.SendRequest] SundaytozResponseHandler is not initialized.");
            onResponseCallback?.Invoke(null);
            return;
        }
        
        if (null == call?.GetAction())
        {
            Debug.LogError("[SundaytozResponseHandler.SendRequest] Action name 이 설정되지 않았습니다.");
            onResponseCallback?.Invoke(null);
            return;
        }
        
        if (null != _responseHandle)
        {
            Debug.LogError("[SundaytozResponseHanler.SendRequest] Already request action. Please try after responsed.");
            onResponseCallback?.Invoke(null);
            return;
        }

        if (null != onResponseCallback)
        {
            var t = new ResponseHandle<T>();
            t.OnResponseCallback = (_t) =>
            {
                StzPluginLogger.Verbose("StzNativeExtension", "ResponseHandler", $"SendRequest<{(typeof(T)?.Name ?? "UNKNOWN")}>", $"End. {(_t?.ToString() ?? "")}");
                LastError.Set(_t);
                onResponseCallback?.Invoke(_t);
            };
            _responseHandle = t;
        }
        
        _plugin.request(call);
    }

    public bool IsReady()
    {
        if (null == _plugin) return false;
        if (null != _responseHandle) return false;

        return true;
    }

    public void AddListener(EStzNativeAction action, ResponseDelegate onResponseCallback)
    {
        var key = action.ToString();

        if (_listenerSet.ContainsKey(key))
            _listenerSet[key] = onResponseCallback;
        else
            _listenerSet.Add(key, onResponseCallback);
    }

    public void RemoveListener(EStzNativeAction action)
    {
        _listenerSet.Remove(action.ToString());
    }
    
    public void RemoveAllListeners()
    {
        _listenerSet.Clear();
    }

    /// <summary>
    /// 네이티브에서 보내준 응답
    /// </summary>
        public void OnResponseCallback(string jsonToken)
    {
        if (TryParseListener(jsonToken)) return;
        
        _responseHandle?.Parse(jsonToken);
        _responseHandle?.Invoke();
        _responseHandle?.Clear();
        _responseHandle = null;
    }

    private bool TryParseListener(string jsonToken)
    {
        var t = JsonUtility.FromJson<StzNativeResult_Base>(jsonToken);

        if (!_listenerSet.TryGetValue(t.Action, out var listener)) return false;
        
        listener?.Invoke(jsonToken);
        
        return true;
    }
}