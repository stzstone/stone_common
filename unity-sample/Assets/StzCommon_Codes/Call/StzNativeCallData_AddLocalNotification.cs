namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_AddLocalNotification : StzNativeCallData_Base
    {
        public int     alarm_id       = 0;
        public int     time           = 0;
        public string  title          = string.Empty;
        public string  message        = string.Empty;
        public int     counter        = 0;
        public int     type           = 0;
        public string  bg_image_name  = string.Empty;
        public string  title_color    = string.Empty;
        public string  msg_color      = string.Empty;
        public int     title_size     = 0;
        public int     msg_size       = 0;

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("action: " + action + ", ")
              .Append("alarm_id: " + alarm_id + ", ")
              .Append("time: " + time + ", ")
              .Append("title: " + title + ", ")
              .Append("message: " + message + ", ")
              .Append("counter: " + counter + ", ")
              .Append("type: " + type + ", ")
              .Append("bg_image_name: " + bg_image_name + ", ")
              .Append("title_color: " + title_color + ", ")
              .Append("msg_color: " + msg_color + ", ")
              .Append("title_size: " + title_size + ", ")
              .Append("msg_size: " + msg_size);

            return sb.ToString();
        }
    }
}