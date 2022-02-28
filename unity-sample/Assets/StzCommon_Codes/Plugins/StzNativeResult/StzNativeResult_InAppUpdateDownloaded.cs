using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_InAppUpdateDownloaded : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public bool is_downloaded = false;
    }

    [SerializeField] private Data data = null;

    public bool IsDownloaded { get { return data?.is_downloaded != null && (bool) data?.is_downloaded; } }
}