using System;
using UnityEngine;

[Serializable]
public class StzNativeResult_Base
{
    //----------------------------------------------------------------
    // variables
    //----------------------------------------------------------------
    [SerializeField] private string action      = string.Empty;
    //[SerializeField] private int    error       = 0;
    [SerializeField] private int    error_code  = 0;
    [SerializeField] private string error_msg   = string.Empty;

    //----------------------------------------------------------------
    // get, set
    //----------------------------------------------------------------
    public string   Action      { get { return action; } }
    public bool     HasError    { get { return 0 != error_code; } }
    public int      ErrorCode   { get { return error_code; } }
    public string   ErrorMsg    { get { return StzNativeResultStringUtil.ParseDataString(error_msg); } }

    public void Set(StzNativeResult_Base data)
    {
        action      = data.action;
        error_code  = data.error_code;
        error_msg   = data.error_msg;
    }

    public override string ToString()
    {
        return $"action: {action}, hasError: {HasError}, errorCode: {ErrorCode}, errorMsg: {ErrorMsg}";
    }
}