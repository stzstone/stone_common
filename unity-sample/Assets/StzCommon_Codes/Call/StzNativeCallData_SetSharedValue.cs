namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SetSharedValue : StzNativeCallData_Base
    {
        public string key   = string.Empty;
        public string value = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, key: {key}, value: {value}";
        }
    }
}