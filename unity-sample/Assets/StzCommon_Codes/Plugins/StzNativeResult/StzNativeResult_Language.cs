using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_Language : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string language = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string Language { get { return StzNativeResultStringUtil.ParseDataString(data?.language); } }
}