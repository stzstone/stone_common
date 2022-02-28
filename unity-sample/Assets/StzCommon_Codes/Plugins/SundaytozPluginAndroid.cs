using UnityEngine;
using STZ_Common;

public class SundaytozPluginAndroid : SundaytozPluginBase
{
	private AndroidJavaObject _pluginInstance;
    private AndroidJavaClass _javaClass;

    /// <summary>
    /// 
    /// </summary>
    public SundaytozPluginAndroid()
    {
		_javaClass = new AndroidJavaClass("com.sundaytoz.plugins.common.SundaytozAndroid");

        string instance = _javaClass.CallStatic<string>("createInstance");
        _pluginInstance = _javaClass.GetStatic<AndroidJavaObject>(instance);
    }
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inCall"></param>
	public override void request(IStzNativeCall inCall)
    {
        _pluginInstance.Call("sendMessageToNative", inCall.GetParamString());
	}

	/// <summary>
	/// 현재 네트워크 상태 (ENetworkType)
	/// </summary>
	/// <returns></returns>
	public override string GetNetworkStatus()
	{
		return _javaClass.CallStatic<string>("getNetworkStatus");
	}
	// /// <summary>
	// /// 키스토어 해시값 획득
	// /// </summary>
	// /// <returns></returns>
	// public override string GetSignature()
	// {
	// 	return _javaClass.CallStatic<string>("a");
	// }

	// /// <summary>
	// /// 현재 퍼미션 획득 상태 (EPermissionGrantType)
	// /// </summary>
	// /// <returns></returns>
	// public override EPermissionGrantType GetPermissionGrantStatus( string permissionName )
	// {
	// 	return (EPermissionGrantType)_javaClass.CallStatic<int>("getPermissionGrantStatus", permissionName );
	// }

	// public override string GetAllPermissions()
	// {
	// 	return _javaClass.CallStatic<string>("getAllPermissions");
	// }

	// /// <summary>
	// /// 앱 인스톨 여부
	// /// </summary>
	// public override bool IsInstalled( string packageName )
	// {
	// 	return _javaClass.CallStatic<bool>("isAppInstalled", packageName);
	// }

	// /// <summary>
	// /// 앱 정보 화면으로 유도
	// /// </summary>
	// public override void showAppDetail()
	// {
	// 	_javaClass.CallStatic("showAppDetail");
	// }
}