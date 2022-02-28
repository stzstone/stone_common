namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SetKeychainValue : StzNativeCallData_Base
    {
        public string value = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, value: {value}";
        }
    }
}