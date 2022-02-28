namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_CancelLocalNotification : StzNativeCallData_Base
    {
        public int  alarm_id  = 0;
        public int  type      = 0;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("action: " + action + ", ")
              .Append("alarm_id: " + alarm_id + ", ")
              .Append("type: " + type);

            return sb.ToString();
        }
    }
}