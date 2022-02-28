namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_Init : StzNativeCallData_Base
    {
        public string main_activity_name = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, activityName: {main_activity_name}";
        }
    }
}