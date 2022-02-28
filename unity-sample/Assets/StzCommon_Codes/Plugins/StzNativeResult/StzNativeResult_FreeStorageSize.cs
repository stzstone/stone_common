using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_FreeStorageSize : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int size = 0;
    }

    [SerializeField] private Data data = null;

    public int Size { get { return data?.size ?? 0; } }
}