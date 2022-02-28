using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StzNativeResult_RequestPermissions : StzNativeResult_Base
{
    [Serializable]
    private class PermissionResult
    {
        public string permission;
        public int status;
        public string message;
    }
    [Serializable]
    private class Data
    {
        public List<PermissionResult> result;
    }

    [SerializeField] private Data data;

    public List<EPermissionGrantType> GrantTypes
    {
        get
        {
            if (data?.result == null || 0 == data.result.Count)
                return null;

            var results = data.result;
            var result   = new List<EPermissionGrantType>();
            var enumType = typeof(EPermissionGrantType);

            for (int i = 0, size = results.Count; i < size; ++i)
            {
                var status = results[i].status;
                
                if (Enum.IsDefined(enumType, status))
                    result.Add((EPermissionGrantType)status);
                else
                    result.Add(EPermissionGrantType.DENIED);
            }

            return result;
        }
    }
}