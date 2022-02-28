using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_SchemeData : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string scheme_data = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string SchemeData { get { return StzNativeResultStringUtil.ParseDataString(data?.scheme_data); } }
}