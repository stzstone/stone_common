using System;

namespace STZ_Common
{
    [Serializable]
    public class StzNativeCallData_GetPermissionGrantType : StzNativeCallData_Base
    {
        public string permission_name = string.Empty;

        public override string ToString()
        {
            return $"aciton: {action}, permissionName: {permission_name}";
        }
    }
}