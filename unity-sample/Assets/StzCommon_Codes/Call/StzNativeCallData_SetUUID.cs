namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SetUUID : StzNativeCallData_Base
    {
        public string value = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, value: {value}";
        }
    }
}