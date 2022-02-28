using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using STZ_Common;
using StzEnums;
using StzNativeExtention;

[STZGSP.STZPluginInfo(pluginName = "StzCommon", version = "0.10.0")]
public class SundaytozNativeExtension : ISundaytozNativeExtension
{
    public static string Version { get { return "0.10.0"; } }

    //----------------------------------------------------------------
    // delegate
    //----------------------------------------------------------------
    /// <summary>인자 없는 콜백 함수</summary>
    public delegate void NoParamCallback();

    /// <summary>네트워크 상태 변화</summary>
    public delegate void OnChangedNetworkStatusHandler(string inNetworkType);

    /// <summary>인앱 업데이트 상태 변화</summary>
    public delegate void OnInAppUpdateHandler(int updateAvaliability, int isAllowedFlexible, int isAllowedImmediate);
    
    /// <summary>인스톨 상태 변화</summary>
    public delegate void OnInstallStateUpdateHandler(int installState);

    //----------------------------------------------------------------
    // singleton
    //----------------------------------------------------------------
    private SundaytozNativeExtension() {}
    private static SundaytozNativeExtension _instance = null;
    public  static SundaytozNativeExtension Instance  
    {
        get
        { 
            StzPluginLogger.RegisterColor("StzNativeExtension", "#569CD6");
            return _instance ?? (_instance = new SundaytozNativeExtension()); 
        }
    }

    //----------------------------------------------------------------
    // delegate
    //----------------------------------------------------------------
    /* 네트워크 상태 변화 */
    public static event OnChangedNetworkStatusHandler OnChangedNetworkStatus;

    public static event OnInAppUpdateHandler OnInAppUpate;
    public static event OnInstallStateUpdateHandler OnInstallStateUpdate;

    public Action OnInAppUpdateDownloaded;

    //----------------------------------------------------------------
    // variables
    //----------------------------------------------------------------
    private EPlatformType _platform = EPlatformType.Unknown;

    private StzNativeDeviceData         _deviceData         = null;
    private StzNativeAppleAccountData   _appleAccountData   = null;

    //----------------------------------------------------------------
    // get, set
    //----------------------------------------------------------------
    public bool               IsInited          { get; private set; } = false;

    public IDeviceData        DeviceData        { get { return _deviceData; } }
    public IAppleAccountData  AppleAccountData  { get { return _appleAccountData; } }

    public bool  IsPlatformIOS      { get { return EPlatformType.iOS == _platform; } }
    public bool  IsPlatformAndroid  { get { return EPlatformType.Android == _platform; } }

    public StzNativeResult_Base LastError { get { return SundaytozResponseHandler.Instance.LastError; } }

    //----------------------------------------------------------------
    // functions
    //----------------------------------------------------------------
    public void Initialize(System.Action<bool> onResponseCallback, string mainActivityClassName, EPlatformType platform)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "Initialize", $"Begin. mainActivityClassName: {mainActivityClassName}, platform: {platform}");

        // 플랫폼
        _platform = platform;

        if (!SundaytozResponseHandler.Instance.InitPlugin(_platform))
        {
            onResponseCallback?.Invoke(false);
            return;
        }

        // 네트워크 상태 변화를 감지하기 위해 
        SundaytozResponseHandler.Instance.RemoveAllListeners();
        SundaytozResponseHandler.Instance.AddListener(EStzNativeAction.ON_CHANGED_NETWORK_STATUS, _OnChangedNetworkStatus);
        
        // 인앱 업데이트 상태 변화 감지를 위해
        SundaytozResponseHandler.Instance.AddListener(EStzNativeAction.IN_APP_UPDATE_DOWNLOAD, _OnInAppUpdateDownLoaded);

        // 디바이스 정보 요청
        var call = new StzNativeCall<StzNativeCallData_Init>(new StzNativeCallData_Init()
        {
            action = EStzNativeAction.INITIALIZE.ToString(),
            main_activity_name = mainActivityClassName,
        });

        SundaytozResponseHandler.Instance.SendRequest<StzNativeDeviceData>(call, (result) =>
        {
            IsInited    = true;
            _deviceData = result;

            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "Initialize", $"End. result: {result}");
            onResponseCallback?.Invoke(!(result?.HasError ?? true));
        });
    }

    /// <summary>
    /// 인스톨 상태가 변화되었을 경우 네이티브에서 호출하는 콜백
    /// </summary>
    private void _OnInstallStateUpdate(string jsonToken)
    {
        var result = JsonUtility.FromJson<StzNativeResult_InstallStateUpdate>(jsonToken);
        if (null == result)
        {
            Debug.LogError($"[SundaytozNativeExtension.OnInstallStateUpdate] Result is null.");
            return;
        }

        OnInstallStateUpdate?.Invoke(result.InstallStatus);
    }

    /// <summary>
    /// 네트워크 상태가 변화되었을 경우 네이티브에서 호출하는 콜백
    /// </summary>
    private void _OnChangedNetworkStatus(string jsonToken)
    {
        var result = JsonUtility.FromJson<StzNativeResult_Network>(jsonToken);
        if (null == result)
        {
            Debug.LogError($"[SundaytozNativeExtension.OnChangeNetworkStatus] Result is null.");
            return;
        }

        OnChangedNetworkStatus?.Invoke(result.NetworkType);
    }

    /// <summary>
    /// InAppUpdate Downloaded 콜백이 오는 콜백
    /// </summary>
    private void _OnInAppUpdateDownLoaded(string jsonToken)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "_OnInAppUpdateDownLoaded", $"Begin. jsonToken: {jsonToken}");
        
        var result = JsonUtility.FromJson<StzNativeResult_InAppUpdateDownloaded>(jsonToken);
        if (null == result)
        {
            Debug.LogError($"[SundaytozNativeExtension.OnChangeNetworkStatus] Result is null.");
            return;
        }

        if (result.IsDownloaded)
            _deviceData.InAppUpdateDownloaded();

        OnInAppUpdateDownloaded?.Invoke();
    }

    /// <summary>
    /// 로컬 푸시 메세지 등록 
    /// </summary>
    public void AddLocalNotification(Action<bool> onResponseCallback,
                                     int inId, int inTimeSec,
                                     string inTitle, string inMessage,
                                     int inCounter, int inType,
                                     string inBgImageName = "",
                                     string inTitleColor = "",
                                     string inMsgColor = "",
                                     int titleSize = 15,
                                     int msgSize = 15)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "AddLocalNotification", "Begin");
        
        var call = new StzNativeCall<StzNativeCallData_AddLocalNotification>(new StzNativeCallData_AddLocalNotification(){
            action        = EStzNativeAction.LOCAL_NOTIFICATION_ADD.ToString(),
            alarm_id      = inId,
            time          = inTimeSec,
            title         = inTitle,
            message       = inMessage,
            counter       = inCounter,
            type          = inType,
            bg_image_name = inBgImageName,
            title_color   = inTitleColor,
            msg_color     = inMsgColor,
            title_size    = titleSize,
            msg_size      = msgSize,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "AddLocalNotification", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "AddLocalNotification", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        }, onResponseCallback == null);
    }

    /// <summary>
    /// 등록된 로컬 푸시 메세지 취소 
    /// </summary>
    public void CancelLocalNotification(Action<bool> onResponseCallback, int inId, int inType)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "CancelLocalNotification", $"Begin");

        var call = new StzNativeCall<StzNativeCallData_CancelLocalNotification>(new StzNativeCallData_CancelLocalNotification()
        {   
            action   = EStzNativeAction.LOCAL_NOTIFICATION_CANCEL.ToString(),
            alarm_id = inId,
            type     = inType,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "CancelLocalNotification", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "CancelLocalNotification", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        }, true);
    }

    /// <summary>
    /// 모든 로컬 푸시 메세지 취소 
    /// </summary>
    public void CancelLocalNotificationAll(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "CancelLocalNotificationAll", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.LOCAL_NOTIFICATION_ALL_CANCEL), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "CancelLocalNotificationAll", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        }, true);
    }

    /// <summary>
    /// 클립보드에 문자열 저장
    /// </summary>
    public void SetTextToSystemClipboard(Action<bool> onResponseCallback, string inText)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetTextToSystemClipboard", $"Begin");

        var call = new StzNativeCall<StzNativeCallData_SetTextToClipboard>(new StzNativeCallData_SetTextToClipboard() 
        {
            action = EStzNativeAction.SET_TEXT_TO_SYSTEM_CLIPBOARD.ToString(),
            text   = inText,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetTextToSystemClipboard", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetTextToSystemClipboard", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// APNS 푸시 등록 
    ///  - iOS 에서만 동작
    /// </summary>
    public void RegistAPNSNotification(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RegistAPNSNotification", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support Only. function: \"RegistAPNSNotification\"");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.APNS_NOTIFICATION_REGIST), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RegistAPNSNotification", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    public void DownloadInAppUpdate(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"Begin");

        if (!IsInited)
        {
            Debug.LogError("[SundaytozNativeExtension.DownloadInAppUpdate] not initialized.");
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"End. result: FALSE");
            onResponseCallback?.Invoke(false);
            return;
        }
        
        switch (DeviceData.InAppUpdateStatus)
        {
            case EInAppUpdate.None:
            case EInAppUpdate.NotAvailable:
                Debug.LogError($"[StzNativeExtension] DownloadInAppUpdate()-> Failed InAppUpdateStatus : {DeviceData.InAppUpdateStatus}");
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"End. result: FALSE");
                onResponseCallback?.Invoke(false);
                break;

            case EInAppUpdate.Available:
                SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_InAppUpdateDownloaded>(new StzNativeCallSimple(EStzNativeAction.IN_APP_UPDATE_DOWNLOAD),
                    (result) =>
                    {
                        var hasError   = result?.HasError ?? true;
                        var downloaded = result?.IsDownloaded ?? false;

                        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"End. reuslt: {hasError}, downloaded: {downloaded}");
                        if (downloaded)
                            _deviceData.InAppUpdateDownloaded();

                        onResponseCallback?.Invoke(downloaded);
                    });
                break;

            case EInAppUpdate.Downloaded:
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"End. result: TRUE");
                onResponseCallback?.Invoke(true);
                break;

            default:
                Debug.LogError($"[StzNativeExtension] DownloadInAppUpdate()-> Unexpected InAppUpdateStatus : {DeviceData.InAppUpdateStatus}");
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "DownloadInAppUpdate", $"End. result: FALSE");
                onResponseCallback?.Invoke(false);
                break;
        }
    }

    public void InstallInAppUpdate(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InstallInAppUpdate", $"Begin");

        if (!IsInited)
        {
            Debug.LogError("[SundaytozNativeExtension.InstallInAppUpdate] not initialized.");
            onResponseCallback?.Invoke(false);
            return;
        }
        
        switch (DeviceData.InAppUpdateStatus)
        {
            case EInAppUpdate.None:
            case EInAppUpdate.NotAvailable:
                Debug.LogError($"[StzNativeExtension] InstallInAppUpdate()-> Failed InAppUpdateStatus : {DeviceData.InAppUpdateStatus}");
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InstallInAppUpdate", $"End. result: FALSE");
                onResponseCallback?.Invoke(false);
                break;
            case EInAppUpdate.Available:
                SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_Base>(new StzNativeCallSimple(EStzNativeAction.IN_APP_UPDATE_INSTALL_IMMEDIATE),
                    (result) =>
                    {
                        var hasError = result?.HasError ?? true;

                        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InstallInAppUpdate", $"End. result: {hasError}");
                        onResponseCallback?.Invoke(hasError);
                    });
                break;
            case EInAppUpdate.Downloaded:
                SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_Base>(new StzNativeCallSimple(EStzNativeAction.IN_APP_UPDATE_INSTALL_FLEXIBLE),
                    (result) =>
                    {
                        var hasError = result?.HasError ?? true;

                        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InstallInAppUpdate", $"End. result: {hasError}");
                        onResponseCallback?.Invoke(hasError);
                    });
                break;
            default:
                Debug.LogError($"[StzNativeExtension] InstallInAppUpdate()-> Unexpected InAppUpdateStatus : {DeviceData.InAppUpdateStatus}");
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InstallInAppUpdate", $"End. result: FALSE");
                onResponseCallback?.Invoke(false);
                break;
        }        
    }

    /// <summary>
    /// 시스템 팝업 출력 
    /// </summary>
    public void ShowAlert(Action<bool, bool> onResponseCallback,
                          string inTitle, string inMessage, string inOkayLabel = null, string inCancelLabel = null)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ShowAlert", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"ShowAlert\"");

        var call = new StzNativeCall<StzNativeCallData_ShowAlert>(new StzNativeCallData_ShowAlert()
        {
            action  = EStzNativeAction.SHOW_ALERT.ToString(),
            title   = inTitle,
            message = inMessage,
            okay    = !string.IsNullOrEmpty(inOkayLabel) && 0 < inOkayLabel.Length ? inOkayLabel : "확인",
            cancel  = !string.IsNullOrEmpty(inCancelLabel) && 0 < inCancelLabel.Length ? inCancelLabel : string.Empty,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ShowAlert", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_ShowAlert>(call, (result) =>
        {
            if (null == result || result.HasError)
            {
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ShowAlert", $"End. result: FALSE");
                onResponseCallback?.Invoke(false, false);
                return;
            }

            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ShowAlert", $"End. result: TRUE, isOk: {result.ButtonIndex}");
            onResponseCallback.Invoke(!result.HasError, 0 == result.ButtonIndex);
        });
    }

    /// <summary>
    /// 푸시 아이디를 가져온다 
    /// </summary>
    public void GetPushId(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPushId", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support Only. function: \"GetPushId\"");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_PushId>(
            new StzNativeCallSimple(EStzNativeAction.GET_PUSH_ID), 
            (response) => 
            {
                if (null == response)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPushId", $"End. response is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPushId", $"End. result: {!response.HasError}, pushId: {response.PushId}");
                onResponseCallback?.Invoke(!response.HasError, response.PushId);
            });
    }

    /// <summary>
    /// 스킴 정보를 가져온다. 
    /// </summary>
    public void GetSchemeData(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSchemeData", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_SchemeData>(
            new StzNativeCallSimple(EStzNativeAction.GET_SCHEME_DATA),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSchemeData", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSchemeData", $"End. reuslt: {!result.HasError}, schemeData: {result.SchemeData}");
                onResponseCallback?.Invoke(!result.HasError, result.SchemeData);
            });
    }

    /// <summary>
    /// 스킴 정보를 클리어 한다.
    /// </summary>
    public void ClearSchemeData(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ClearSchemeData", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.CLEAR_SCHEME_DATA), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ClearSchemeData", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 현재 네트워크 상태를 NOT_CONNECTED, WIFI, MOBILE로 구분해 반환 
    /// </summary>
    /// <returns></returns>
    public string GetNetworkStatus()
    {
        //StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"Begin");
        
        var result = SundaytozResponseHandler.Instance.GetNetworkStatus();

        //StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"End. status: {result}");
        return result;
    }
    
    /// <summary>
    /// 현재 네트워크 상태를 NOT_CONNECTED, WIFI, MOBILE로 구분해 반환 
    /// </summary>
    public void GetNetworkStatus(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"Begin");

        switch (_platform)
        {
        case EPlatformType.UnityEditor:
        case EPlatformType.iOS:
            {
                var result = _GetNetworkStatus();
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"End. status: {result}");
                onResponseCallback?.Invoke(true, result);
            }
            break;

        case EPlatformType.Android:
            SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_Network>(
                new StzNativeCallSimple(EStzNativeAction.GET_NETWORK_STATUS),
                (result) =>
                {
                    if (null == result)
                    {
                        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"End. result is null. FALSE");
                        onResponseCallback?.Invoke(false, string.Empty);
                        return;
                    }

                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetNetworkStatus", $"End. result: {!result.HasError}, status: {result.NetworkType}");
                    onResponseCallback?.Invoke(!result.HasError, result.NetworkType);
                });
            break;

        default:
            throw new StzNativeNotSupportException("Invalid platform.");
        }
    }

    private string _GetNetworkStatus()
    {
        switch(UnityEngine.Application.internetReachability)
        {
        case UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork:
            return ENetworkType.MOBILE;
        
        case UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork:
            return ENetworkType.WIFI;

        default:
            return ENetworkType.NOT_CONNECTED;
        }
    }

    /// <summary>
    /// 키.해. 값 을 가져옵니다.
    ///  - Android 에서만 동작합니다.
    /// </summary>
    public void GetSignature(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSignature", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"GetSignature\"");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_STimer>(
            new StzNativeCallSimple(EStzNativeAction.GET_STIMER),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSignature", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSignature", $"End. result: {!result.HasError}, STimer: {result.STimer}");
                onResponseCallback?.Invoke(!result.HasError, result.STimer);
            });
    }

    /// <summary>
    /// 앱 인스톨 여부 확인
    /// </summary>
    public void IsInstalled(Action<bool, bool> onResponseCallback, string packageName)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "IsInstalled", $"Begin");

        var call = new StzNativeCall<StzNativeCallData_IsInstalled>(new StzNativeCallData_IsInstalled()
        {
            action       = EStzNativeAction.IS_INSTALLED.ToString(),
            package_name = packageName,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "IsInstalled", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_Installed>(call,
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "IsInstalled", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, false);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "IsInstalled", $"End. result: {!result.HasError}, installed: {result.IsInstalled}");
                onResponseCallback?.Invoke(!result.HasError, result.IsInstalled);
            });
    }

    /// <summary>
    /// 현재 퍼미션 획득 상태를 가져온다.
    ///  - Android 에서만 동작합니다.
    /// </summary>
    public void GetPermissionGrantType(Action<bool, EPermissionGrantType> onResponseCallback, string permissionName)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPermissionGrantType", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"GetPermissionGrantType\"");

        var call = new StzNativeCall<StzNativeCallData_GetPermissionGrantType>(new StzNativeCallData_GetPermissionGrantType()
        {
            action          = EStzNativeAction.GET_PERMISSION_GRANT_TYPE.ToString(),
            permission_name = permissionName,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPermissionGrantType", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_PermissionGrantType>(call,
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPermissionGrantType", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, EPermissionGrantType.DENIED);
                    return;
                }

                var hasError  = result?.HasError ?? true;
                var grantType = result?.GrantType ?? EPermissionGrantType.DENIED;

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetPermissionGrantType", $"End. result: {hasError}, grant: {grantType}");
                onResponseCallback?.Invoke(!hasError, grantType);
            });
    }

    /// <summary>
    /// 획득하지 못한 퍼미션을 모두 가져온다.
    ///  - Android 에서만 동작합니다.
    /// </summary>
    public void GetAllUngrantedPermissions(Action<bool, List<string>> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAllUngrantedPermissions", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"GetAllUngrantedPermissions\"");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_AllUngrantedPermissions>(
            new StzNativeCallSimple(EStzNativeAction.GET_ALL_UNGRANTED_PERMISSIONS),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAllUngrantedPermissions", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, new List<string>());
                    return;
                }

                var sb = new System.Text.StringBuilder();
                sb.Append("permissions: [");

                if (null != result.Permissions && !result.HasError)
                {
                    for (int i = 0, size = result.Permissions.Count; i < size; ++i)
                    {
                        if (0 < i)
                            sb.Append(", ");
                        sb.Append(result.Permissions[i]);
                    }
                }

                sb.Append("]");

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAllUngrantedPermissions", $"End. result: {!result.HasError}, {sb.ToString()}");
                onResponseCallback?.Invoke(!result.HasError, result.Permissions);
            });
    }

    /// <summary>
    /// 퍼미션을 요청한다.
    ///  - Android 에서만 동작합니다.
    /// </summary>
    public void RequestPermission(Action<bool, List<EPermissionGrantType>> onResponseCallback, List<string> permissionNames)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestPermission", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"RequestPermissions\"");

        var call = new StzNativeCall<StzNativeCallData_RequestPermissions>(new StzNativeCallData_RequestPermissions()
        {
            action      = EStzNativeAction.REQUEST_PERMISSIONS.ToString(),
            permissions = permissionNames,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestPermission", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_RequestPermissions>(call,
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestPermission", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, new List<EPermissionGrantType>());
                    return;
                }

                var sb = new System.Text.StringBuilder();
                sb.Append("grantTypes: [");

                if (!result.HasError && null != result.GrantTypes)
                {
                    for (int i = 0, size = result.GrantTypes.Count; i < size; ++i)
                    {
                        if (0 < i)
                            sb.Append(", ");
                        sb.Append(result.GrantTypes[i].ToString());
                    }
                }

                sb.Append("]");

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestPermission", $"End. result: {!result.HasError}, {sb.ToString()}");
                onResponseCallback?.Invoke(!result.HasError, result.GrantTypes);
            });
    }

    /// <summary>
    /// 설정화면으로 이동합니다.
    /// </summary>
    public void OpenAppDetail(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "OpenAppDetail", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.OPEN_APP_DETAIL), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "OpenAppDetail", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 인스톨 유입 경로를 제거 합니다.
    ///  - Android 에서만 동작합니다.
    /// </summary>
    public void ClearInstallReferrer(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ClearInstallReferrer", $"Begin");

        if (!IsPlatformAndroid)
            throw new StzNativeNotSupportException("Android Support Only. function: \"ClearInstallReferrer\"");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.CLEAR_INSTALL_REFERRER), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "ClearInstallReferrer", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 프로젝트의 고유키로 값을 가져온다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void GetKeychainValue(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetKeychainValue", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"GetKeychainValue\"");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_KeychainValue>(
            new StzNativeCallSimple(EStzNativeAction.GET_KEYCHAIN_VALUE),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetKeychainValue", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetKeychainValue", $"End. result: {!result.HasError}, value: {result.Value}");
                onResponseCallback?.Invoke(!result.HasError, result.Value);
            });
    }

    /// <summary>
    /// 프로젝트의 고유키에 값을 저장한다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void SetKeychainValue(Action<bool> onResponseCallback, string value)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetKeychainValue", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"GetKeychainValue\"");

        var call = new StzNativeCall<StzNativeCallData_SetKeychainValue>(new StzNativeCallData_SetKeychainValue() 
        {
            action = EStzNativeAction.SET_KEYCHAIN_VALUE.ToString(),
            value  = value,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetKeychainValue", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetKeychainValue", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }
    
    /// <summary>
    /// 특정 키로 입력된 값을 가져온다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void GetSharedValue(Action<bool, string> onResponseCallback, string key, string bundleId = "")
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSharedValue", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"GetSharedValue\"");

        if (string.IsNullOrEmpty(bundleId))
            bundleId = Application.identifier;

        var call = new StzNativeCall<StzNativeCallData_GetSharedValue>(new StzNativeCallData_GetSharedValue() 
        {
            action = EStzNativeAction.GET_SHARED_VALUE.ToString(),
            key    = bundleId + "_" + key,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSharedValue", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_KeychainValue>(call,
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSharedValue", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetSharedValue", $"End. result: {!result.HasError}, value: {result.Value}");
                onResponseCallback?.Invoke(!result.HasError, result.Value);
            });
    }

    /// <summary>
    /// 특정 키에 값을 입력한다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void SetSharedValue(Action<bool> onResponseCallback, string key, string value)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetSharedValue", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"SetSharedValue\"");

        var call = new StzNativeCall<StzNativeCallData_SetSharedValue>(new StzNativeCallData_SetSharedValue()
        {
            action = EStzNativeAction.SET_SHARED_VALUE.ToString(),
            key    = Application.identifier + "_" + key,
            value  = value,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetSharedValue", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetSharedValue", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 기기의 로딩 화면을 노출시킨다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void StartDeviceLoading(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "StartDeviceLoading", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"StartDeviceLoading\"");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.START_DEVICE_LOADING), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "StartDeviceLoading", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 기기의 로딩 화면을 끈다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void StopDeviceLoading(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "StopDeviceLoading", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"StopDeviceLoading\"");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.STOP_DEVICE_LOADING), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "StopDeviceLoading", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }
    
    /// <summary>
    /// 애플 계정을 초기화 한다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void InitAppleAccount(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InitAppleAccount", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"InitAppleAccount\"");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeAppleAccountData>(
            new StzNativeCallSimple(EStzNativeAction.INIT_APPLE_ACCOUNT),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InitAppleAccount", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false);
                    return;
                }

                _appleAccountData = result;
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "InitAppleAccount", $"End. result: {!result.HasError}");
                onResponseCallback?.Invoke(!result.HasError);
            });
    }

    /// <summary>
    /// 애플 계정에 로그인 합니다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void LoginAppleAccount(Action<bool> onResponseCallback, string profileScope)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"Begin");

        var version = new Version();
#if UNITY_IOS
        version = new Version(UnityEngine.iOS.Device.systemVersion);
#endif
        if(version.Major < 13 )
            throw new StzNativeNotSupportException($"iOS OS 13 이상만 지원합니다. 현재 os : {version.Major}, function: \"LoginAppleAccount\"");
        
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"LoginAppleAccount\"");

        if (null == AppleAccountData)
        {
            Debug.LogError("[SundaytozNativeExtension.LoginAppleAccount] Not initialize AppleAccountSystem. Please call InitAppleAccount first.");
            onResponseCallback?.Invoke(false);
        }

        switch (AppleAccountData.Status)
        {
        case EAppleAccountStatus.LoggedIn:
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"End. result: TRUE");
            onResponseCallback?.Invoke(true);
            return;

//        case EAppleAccountStatus.ImpossibleLogin:
//        case EAppleAccountStatus.Unknown:
//            Debug.LogWarning("[SundaytozNativeExtension.LoginAppleAccount] Invalid status.");
//            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"End. result: FALSE, status: {AppleAccountData.Status}");
//            onResponseCallback?.Invoke(false);
//            return;
        }

        var call = new StzNativeCall<StzNativeCallData_LoginAppleAccount>(new StzNativeCallData_LoginAppleAccount()
        {
            action       = EStzNativeAction.LOGIN_APPLE_ACCOUNT.ToString(),
            profile_scope = profileScope,
        });

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"Call | {call.ToString()}");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_LoginAppleAccount>(call,
            result =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false);
                    return;
                }

                _appleAccountData.Set(result);
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LoginAppleAccount", $"End. status: {AppleAccountData.Status}");
                onResponseCallback?.Invoke(EAppleAccountStatus.LoggedIn == AppleAccountData.Status);
            });        
    }

    /// <summary>
    /// 애플 계정에서 로그아웃한다.
    ///  - iOS 에서만 동작합니다.
    /// </summary>
    public void LogoutAppleAccount(Action<bool> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LogoutAppleAccount", $"Begin");

        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"LogoutAppleAccount\"");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.LOGOUT_APPLE_ACCOUNT), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "LogoutAppleAccount", $"End. result: {_success}");
            onResponseCallback?.Invoke(_success);
        });
    }

    /// <summary>
    /// 언어 정보를 가져온다.
    /// </summary>
    public void GetLanguage(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetLanguage", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_Language>(
            new StzNativeCallSimple(EStzNativeAction.GET_LANGUAGE),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetLanguage", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetLanguage", $"End. result: {!result.HasError}, language: {result.Language}");
                onResponseCallback?.Invoke(!result.HasError, result.Language);
            });
    }

    /// <summary>
    /// 사용 가능한 저장공간의 크기를 가져온다. (단위: Mb)
    /// </summary>
    public void GetFreeSpaceMb(Action<bool, int> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetFreeSpaceMb", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_FreeStorageSize>(
            new StzNativeCallSimple(EStzNativeAction.GET_FREE_SPACE_MB),
            (result) => 
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetFreeSpaceMb", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, 0);
                    return;
                }

                if (!result.HasError)
                    _deviceData?.SetFreeSpaceMb(result.Size);

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetFreeSpaceMb", $"End. result: {!result.HasError}, size: {result.Size}");
                onResponseCallback?.Invoke(!result.HasError, result.Size);
            });
    }
    
    /// <summary>
    /// 강제로 크래시를 발생시킨다. 
    /// </summary>
    public void RiseCrash()
    {
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"RiseCrash\"");

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RiseCrash", $"Begin");

        SundaytozResponseHandler.Instance.SendRequest(new StzNativeCallSimple(EStzNativeAction.RISE_CRASH), (_success) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RiseCrash", $"End. result: {_success}");
        });
    }

    /// <summary>
    /// 리치 푸시에 사용될 예약어를 저장한다.
    /// </summary>
    /// <param name="onResponseCallback">콜백</param>
    /// <param name="replaceKey">예약어 키</param>
    /// <param name="replaceValue">예약어 값</param>
    public void SetReservedWord(Action<bool> onResponseCallback, string replaceKey, string replaceValue, string groupId = "group.com.sundaytoz")
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetReplaceWord", "Begin");

        var stzNativeCall = new StzNativeCall<StzNativeCallData_SetReservedWord>(new StzNativeCallData_SetReservedWord()
        {
            action = EStzNativeAction.SET_RESERVED_WORD.ToString(),
            word_key = replaceKey,
            word_value =  replaceValue,
            group_id =  groupId
        });

        SundaytozResponseHandler.Instance.SendRequest(stzNativeCall, (isSuccess) => { onResponseCallback?.Invoke(isSuccess); });
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetReplaceWord", "End");
    }

    public void GetAndClearDeeplink(Action<bool, string> onResponseCallback)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAndClearDeeplink", "Begin");
        
        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_DeepLink>(new StzNativeCallSimple(EStzNativeAction.GET_AND_CLEAR_DEEPLINK),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAndClearDeeplink", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, string.Empty);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAndClearDeeplink", $"End. result: {!result.HasError}, deeplink: {result.Deeplink}");
                onResponseCallback?.Invoke(!result.HasError, result.Deeplink);
            });
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAndClearDeeplink", "End");
    }

    public void GetAttStatus(Action<bool, EATTrackingManagerAuthorizationStatus> onResponseCallback)
    {
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"GetAttStatus\"");
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAttStatus", "Begin");
        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_GetAttStatus>(new StzNativeCallSimple(EStzNativeAction.GET_ATT_STATUS),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAttStatus", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, EATTrackingManagerAuthorizationStatus.ATTrackingManagerAuthorizationStatusNotDetermined);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetDeepLinkFromPush", $"End. result: {!result.HasError}, status: {result.Status}");
                onResponseCallback?.Invoke(!result.HasError, result.Status);
            });
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetAttStatus", "End");
    }
    
    public void RequestAtt(Action<bool, EATTrackingManagerAuthorizationStatus> onResponseCallback)
    {
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"RequestAtt\"");
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestAtt", "Begin");
        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_RequestAtt>(new StzNativeCallSimple(EStzNativeAction.REQUEST_ATT),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestAtt", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, EATTrackingManagerAuthorizationStatus.ATTrackingManagerAuthorizationStatusNotDetermined);
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetDeepLinkFromPush", $"End. result: {!result.HasError}, status: {result.Status}");
                onResponseCallback?.Invoke(!result.HasError, result.Status);
            });
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "RequestAtt", "End");
    }
    
    public void GetIDFA(Action<bool, string> onResponseCallback)
    {
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"GetIDFA\"");
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetIDFA", "Begin");
        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_GetIDFA>(new StzNativeCallSimple(EStzNativeAction.GET_IDFA),
            (result) =>
            {
                if (null == result)
                {
                    StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetIDFA", $"End. result is null. FALSE");
                    onResponseCallback?.Invoke(false, "");
                    return;
                }

                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetIDFA", $"End. result: {!result.HasError}, IDFA: {result.IDFA}");
                onResponseCallback?.Invoke(!result.HasError, result.IDFA);
            });
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "GetIDFA", "End");
    }

    public bool IsReady()
    {
        bool isReady = IsInited && SundaytozResponseHandler.Instance.IsReady();

        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "IsReady", $"IsReady : {isReady}");
        return isReady;
    }
    
    public void SetUUID(Action<bool> onResponseCallback, string uuid)
    {
        if (!IsPlatformIOS)
            throw new StzNativeNotSupportException("iOS Support only. function: \"SetUUID\"");

        if (_deviceData.UUID == uuid)
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetUUID", "Already same value uuid.");
            onResponseCallback(true);
        }
        else
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetUUID", "Begin");
            var call = new StzNativeCall<StzNativeCallData_SetUUID>(new StzNativeCallData_SetUUID() 
            {
                action = EStzNativeAction.SET_UUID.ToString(),
                value  = uuid,
            });

            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetUUID", $"Call | {call}");

            SundaytozResponseHandler.Instance.SendRequest(call, (_success) =>
            {
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetUUID", $"End. result: {_success}");
                onResponseCallback?.Invoke(_success);
            });
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SetUUID", "End");
        }
    }
    
    public void SendEmail(Action<bool> onResponseCallback, string receiverAddress, string title, string content)
    {
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SendEmail", "Begin");

        var call = new StzNativeCall<StzNativeCallData_SendEmail>(new StzNativeCallData_SendEmail()
        {
            action       = EStzNativeAction.SEND_EMAIL.ToString(),
            receiver_address = receiverAddress,
            title =  title,
            body = content

        });
        
        SundaytozResponseHandler.Instance.SendRequest<StzNativeResult_SendEmail>(call, (result) =>
        {
            StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SendEmail", $"Begin");
            if (result == null)
            {
                StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SendEmail", $"result is null");
                onResponseCallback?.Invoke(false);
                return;
            }
            onResponseCallback?.Invoke(result.Success);
        });
        
        StzPluginLogger.Verbose("StzNativeExtension", "StzNativeExtension", "SendEmail", "End");
    }
}