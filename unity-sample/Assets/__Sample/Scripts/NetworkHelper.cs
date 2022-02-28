using System;
using System.Text;

public class NetworkHelper
{
    /// <summary>
    /// 네트워크 통신이 가능한 상태인지 여부 
    /// </summary>
    /// <returns></returns>
    public static bool IsOnline()
    {
        return true;
        //return (SundaytozNativeExtension.Instance.GetNetworkStatus().CompareTo(ENetworkType.NOT_CONNECTED) != 0);
    }
}
