using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_Network : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string network_type = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string NetworkType { get { return StzNativeResultStringUtil.ParseDataString(data?.network_type); } }
}