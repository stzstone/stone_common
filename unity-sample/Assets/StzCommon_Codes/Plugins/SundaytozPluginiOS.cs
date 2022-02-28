using System.Runtime.InteropServices;
using UnityEngine;
using STZ_Common;

#if UNITY_IOS
public class SundaytozPluginiOS : SundaytozPluginBase
{
    [DllImport("__Internal")]
    private static extern void sundaytozUnityExtension(string action, ResponseCallback callback);
    private delegate void ResponseCallback(string response);

    /// <summary>
    /// 
    /// </summary>
    public SundaytozPluginiOS()
    {
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hasError"></param>
    /// <param name="response"></param>
    [AOT.MonoPInvokeCallback(typeof(ResponseCallback))]
    private static void handleResponseCallback(string response)
    {
        SundaytozResponseHandler.Instance.OnResponseCallback(response);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inCall"></param>
    public override void request(IStzNativeCall inCall)
    {
        sundaytozUnityExtension(inCall.GetParamString(), handleResponseCallback);
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
             case UnityEngine.NetworkReachability.NotReachable:
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
}
#endif
