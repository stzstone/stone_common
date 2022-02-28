using System;
using StzEnums;

namespace StzNativeExtention
{
    public interface ISundaytozNativeExtension
    {
        //----------------------------------------------------------------
        // get ,set
        //----------------------------------------------------------------
        IDeviceData DeviceData { get; }
        IAppleAccountData AppleAccountData { get; }

        //----------------------------------------------------------------
        // interface
        //----------------------------------------------------------------
        void Initialize(Action<bool> onResponseCallback, string mainActivityClassName, EPlatformType platform);

        string GetNetworkStatus();
        void GetNetworkStatus(Action<bool, string> onResponseCallback);

        void CancelLocalNotificationAll(Action<bool> onResponseCallback);

        void InitAppleAccount(Action<bool> onResponseCallback);
        void LoginAppleAccount(Action<bool> onResponseCallback, string profileScope);
        void LogoutAppleAccount(Action<bool> onResponseCallback);

        void SetSharedValue(Action<bool> onResponseCallback, string key, string value);
        void GetSharedValue(Action<bool, string> onResponseCallback, string key, string bundleIdentifier);
        void SetUUID(Action<bool> onResponseCallback, string uuid);
    }
}