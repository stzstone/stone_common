using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_PushId : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public string registration_id = string.Empty;
    }

    [SerializeField] private Data data = null;

    public string PushId { get { return StzNativeResultStringUtil.ParseDataString(data?.registration_id); } }
}