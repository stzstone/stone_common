namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_ShowAlert : StzNativeCallData_Base
    {
        public string  title    = string.Empty;
        public string  message  = string.Empty;
        public string  okay     = string.Empty;
        public string  cancel   = string.Empty;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("action: " + action + ", ")
              .Append("title: " + title + ", ")
              .Append("message: " + message + ", ")
              .Append("okay: " + okay + ", ")
              .Append("cancel: " + cancel);

            return sb.ToString();
        }
    }
}