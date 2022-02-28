using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_PermissionGrantType : StzNativeResult_Base
{
    [Serializable]
    private class Data
    {
        public int status = -1;
    }

    [SerializeField] private Data data = null;

    public EPermissionGrantType GrantType
    {
        get
        {
            if (null == data || !Enum.IsDefined(typeof(EPermissionGrantType), data.status))
                return EPermissionGrantType.DENIED;

            return (EPermissionGrantType)data.status;
        }
    }
}