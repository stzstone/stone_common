using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_GetIDFA : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string idfa = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string IDFA { get { return StzNativeResultStringUtil.ParseDataString(data?.idfa); } }
}