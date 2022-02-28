using System;
using UnityEngine;
using StzNativeExtention;

[Serializable]
public sealed class StzNativeAppleAccountData : StzNativeResult_Base, IAppleAccountData
{
    public enum EStatus
    {
        Unknown             = -1,
        Revoked             = 0,
        LoggedIn            = 1,
        CredentialNotFound  = 2,
        Transferred         = 3,
        ImpossibleLogin     = 4,
    }

    [Serializable]
    private class Data
    {
        public int     status          = -1;
        public string  detail_msg      = string.Empty;
        public string  id              = string.Empty;
        public string  email           = string.Empty;
        public string  user_full_name  = string.Empty;
    }

    [SerializeField] private Data data = null;

    public EAppleAccountStatus Status
    {
        get
        {
            if (null == data || !Enum.IsDefined(typeof(EAppleAccountStatus), data.status))
                return EAppleAccountStatus.Unknown;

            return (EAppleAccountStatus)data.status;
        }
    }

    public string StatusDetailMessage   { get { return ParseDataString(data?.detail_msg); } }
    public string Id                    { get { return ParseDataString(data?.id); } }
    public string Email                 { get { return ParseDataString(data?.email); } }
    public string UserFullName          { get { return ParseDataString(data?.user_full_name); } }

    public void Set(StzNativeResult_LoginAppleAccount result)
    {
        if (null == result)
            return;

        if (result.HasError)
        {
            data.status         = (int)EStatus.ImpossibleLogin;
            data.detail_msg     = string.Empty;
            data.id             = string.Empty;
            data.email          = string.Empty;
            data.user_full_name = string.Empty;
            return;
        }

        data.status         = (int)EStatus.LoggedIn;
        data.detail_msg     = string.Empty;
        data.id             = result.Id;
        data.email          = result.Email;
        data.user_full_name = result.UserFullName;
    }

    private string ParseDataString(string s)
    {
        if (string.IsNullOrEmpty(s) || "empty" == s)
            return string.Empty;

        return s;
    }
}