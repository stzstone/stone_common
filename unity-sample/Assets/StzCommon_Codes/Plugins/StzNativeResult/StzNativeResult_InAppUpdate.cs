using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_InAppUpdate : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public bool isSuccess;
    }

    [SerializeField] private Data data = null;
    
    public bool IsSuccess => data?.isSuccess != null && (bool) data?.isSuccess;
}