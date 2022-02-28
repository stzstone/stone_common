using System;
using UnityEngine;

public class StzNativeResult_ShowAlert : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int button = -1;
    }

    [SerializeField] private Data data = null;

    public int ButtonIndex { get { return data?.button ?? -1; } }
}