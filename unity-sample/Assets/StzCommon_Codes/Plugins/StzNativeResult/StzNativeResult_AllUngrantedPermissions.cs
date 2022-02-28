using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StzNativeResult_AllUngrantedPermissions : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public List<string> permissions = null;
    }

    [SerializeField] private Data data = null;

    public List<string> Permissions { get { return data?.permissions ?? null; } }
}