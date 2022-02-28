using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_DeepLink : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string deeplink = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string Deeplink { get { return StzNativeResultStringUtil.ParseDataString(data?.deeplink); } }
}