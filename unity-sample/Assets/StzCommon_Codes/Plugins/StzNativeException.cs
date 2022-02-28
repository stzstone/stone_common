using System;

[Serializable]
public class StzNativeNotSupportException : System.Exception
{
    public StzNativeNotSupportException() : base() {}
    public StzNativeNotSupportException(string message) : base(message) {}
    public StzNativeNotSupportException(string message, Exception inner) : base(message, inner) {}
}