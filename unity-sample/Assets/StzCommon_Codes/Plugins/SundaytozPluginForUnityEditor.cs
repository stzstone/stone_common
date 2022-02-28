using UnityEngine;
using STZ_Common;

public class SundaytozPluginForUnityEditor : SundaytozPluginBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inCall"></param>
    public override void request(IStzNativeCall inCall)
    {
        SundaytozResponseHandler.Instance.OnResponseCallback("{\"action\":\"" + inCall.GetAction() + "\",\"error\":false,\"error_code\":0,\"error_msg\":\"empty\",\"data\":{}}");

        //var action = inCall.GetAction();

        //JSONClass json = new JSONClass();
        //json["action"] = action;

        //JSONClass result = new JSONClass();
        //result["status"] = "0";

        //json.Add("result", result);

        //SundaytozResponseHandler.Instance.OnResponseCallback(json.ToString());
    }

    /// <summary>
    /// 현재 네트워크 상태 (ENetworkType)
    /// </summary>
    /// <returns></returns>
    public override string GetNetworkStatus()
    {
        string status = "";

        switch(UnityEngine.Application.internetReachability)
        {
            default:
            {
                status = ENetworkType.NOT_CONNECTED;
                break;
            }

            case UnityEngine.NetworkReachability.ReachableViaCarrierDataNetwork:
            {
                status = ENetworkType.MOBILE;
                break;
            }

            case UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork:
            {
                status = ENetworkType.WIFI;
                break;
            }
        }

        return status;
    }
	// /// <summary>
	// /// 키스토어 해시값 획득
	// /// </summary>
	// /// <returns></returns>
	// public override string GetSignature()
	// {
	// 	return string.Empty;
	// }

	// public override EPermissionGrantType GetPermissionGrantStatus (string permissionName)
	// {
	// 	return EPermissionGrantType.ALREADY_GRANTED;
	// }

    // public override string GetAllPermissions()
    // {
    //     return string.Empty;
    // }

    // public override void showAppDetail ()
	// {
	// }

    // public override bool IsInstalled(string packageName)
    // {
    //     return true;
    // }
}