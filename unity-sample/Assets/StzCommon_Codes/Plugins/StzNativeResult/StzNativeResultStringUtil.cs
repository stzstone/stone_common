public static class StzNativeResultStringUtil
{
    public static string ParseDataString(string s)
    {
        if (string.IsNullOrEmpty(s) || s == "empty")
            return string.Empty;
        return s;
    }
}