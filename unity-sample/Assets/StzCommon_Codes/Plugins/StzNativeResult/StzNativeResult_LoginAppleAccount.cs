using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_LoginAppleAccount : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string  id              = string.Empty;
        public string  email           = string.Empty;
        public string  user_full_name  = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string Id            { get { return StzNativeResultStringUtil.ParseDataString(data?.id); } }
    public string Email         { get { return StzNativeResultStringUtil.ParseDataString(data?.email); } }
    public string UserFullName  { get { return StzNativeResultStringUtil.ParseDataString(data?.user_full_name); } }
}