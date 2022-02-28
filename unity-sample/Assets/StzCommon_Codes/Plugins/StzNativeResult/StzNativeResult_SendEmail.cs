using System;
using UnityEngine;

public class StzNativeResult_SendEmail : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public bool success = false;
    }

    [SerializeField] private Data data = null;

    public bool Success { get { return data.success; } }
}