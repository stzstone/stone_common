namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SetTextToClipboard : StzNativeCallData_Base
    {
        public string text = string.Empty;

        public override string ToString()
        {
            return $"action: {action}, text: {text}";
        }
    }
}