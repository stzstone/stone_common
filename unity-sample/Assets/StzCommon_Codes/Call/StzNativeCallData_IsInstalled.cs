namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_IsInstalled : StzNativeCallData_Base
    {
        public string package_name = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, packageName: {package_name}";
        }
    }
}