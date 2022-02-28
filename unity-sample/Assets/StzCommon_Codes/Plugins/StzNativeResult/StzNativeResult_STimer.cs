using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_STimer : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string stimer = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string STimer { get { return StzNativeResultStringUtil.ParseDataString(data?.stimer); } }
}