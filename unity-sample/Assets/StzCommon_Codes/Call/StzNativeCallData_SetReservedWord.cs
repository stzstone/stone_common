namespace STZ_Common
{
    [System.Serializable]
    public class StzNativeCallData_SetReservedWord : StzNativeCallData_Base
    {
        public string word_key = string.Empty;
        public string word_value = string.Empty;
        public string group_id = string.Empty;

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(word_key)}: {word_key}, {nameof(word_value)}: {word_value}, {nameof(group_id)}: {group_id}";
        }
    }
}