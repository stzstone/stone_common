using System.Collections.Generic;

namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_RequestPermissions : StzNativeCallData_Base
    {
        public List<string> permissions;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("action: " + action + ", ");

            if (null != permissions)
            {
                sb.Append("permissions: [");
                for (int i = 0, size = permissions.Count; i < size; ++i)
                {
                    if (0 < i)
                        sb.Append(", ");
                    sb.Append(permissions[i]);
                }
                sb.Append("]");
            }

            return sb.ToString();
        }
    }
}