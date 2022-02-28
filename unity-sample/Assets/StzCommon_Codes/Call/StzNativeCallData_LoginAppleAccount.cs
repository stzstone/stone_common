namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_LoginAppleAccount : StzNativeCallData_Base
    {
        public string profile_scope = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, profileScope: {profile_scope}";
        }
    }
}