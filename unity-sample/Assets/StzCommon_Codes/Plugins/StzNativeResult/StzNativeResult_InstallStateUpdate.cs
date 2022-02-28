using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_InstallStateUpdate : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int install_status;
    }

    [SerializeField] private Data data = null;
    
    public int InstallStatus => data?.install_status ?? -1;
}