using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_Installed : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public bool installed = false;
    }

    [SerializeField] private Data data = null;

    public bool IsInstalled { get { return (data?.installed ?? false); } }
}