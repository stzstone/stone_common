using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_KeychainValue : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string value = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string Value { get { return StzNativeResultStringUtil.ParseDataString(data?.value); } }
}