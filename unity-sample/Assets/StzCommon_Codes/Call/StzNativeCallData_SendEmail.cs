namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SendEmail : StzNativeCallData_Base
    {
        public string receiver_address = "";
        public string title = "";
        public string body = "";
        
        public override string ToString()
        {
            return $"action:{action}, {base.ToString()}, {nameof(receiver_address)}: {receiver_address}, {nameof(title)}: {title}, {nameof(body)}: {body}";
        }
    }
}