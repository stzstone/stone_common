namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_GetSharedValue : StzNativeCallData_Base
    {
        public string key = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, key: {key}";
        }
    }
}