/*
 * Copyright (C) SundayToz Corp. All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
*/

﻿using StzEnums;
using System;
using System.Collections.Generic;
using System.IO;

#if UNITY_2018
[STZGSP.STZPluginInfo(pluginName = "StzPluginLogger", version = "2018.0.6")]
#else
[STZGSP.STZPluginInfo(pluginName = "StzPluginLogger", version = "2019.0.6")]
#endif
public static class StzPluginLogger
{
    public enum ELogLevel
    {
        Verbose       = 0,
        Warning       = 1,
        Error         = 2,
        ErrorToServer = 3,
    }

    //----------------------------------------------------------------
    // variables
    //----------------------------------------------------------------
    private static Dictionary<string, string>  _colors               = new Dictionary<string, string>();
    private static string                      _defaultColor         = "#999999";
    private static Action<string>              _onSendErrorToServer  = null;

    //----------------------------------------------------------------
    // get, set
    //----------------------------------------------------------------
    public static ELogLevel         LogLevel            { get; set; } = ELogLevel.Verbose;
    public static EPlatformType     PlatformType        { get; set; } = EPlatformType.Unknown;
    public static bool              ErrorToServerOnly   { get; set; } = false;
    public static bool              EnableFileLog       { get; set; } = false;

    private static string FILE_LOG_PATH 
    {
        get 
        { 
            if (EPlatformType.UnityEditor == PlatformType)
                return UnityEngine.Application.persistentDataPath + "/StzPluginLog.txt";
            return UnityEngine.Application.dataPath + "/StzPluginLog.txt"; 
        } 
    }

    //----------------------------------------------------------------
    // functions
    //----------------------------------------------------------------
    public static void RegisterColor(string pluginName, string color)
    {
        UnregisterColor(pluginName);
        
        if (color[0] != '#')
            color = "#" + color;
        if (7 < color.Length)
            color = color.Substring(0, 7);

        _colors.Add(pluginName, color);
    }

    public static void UnregisterColor(string pluginName)
    {
        if (_colors.ContainsKey(pluginName))
            _colors.Remove(pluginName);
    }

    public static void Verbose(string pluginName, string className, string funcName, string msg)
    {
        if ((int)ELogLevel.Verbose < (int)LogLevel || EPlatformType.Unknown == PlatformType)
            return;

        if (EPlatformType.UnityEditor == PlatformType)
        {
            var color = GetColor(pluginName);
            UnityEngine.Debug.Log($"<color={color}>[{pluginName}.{className}.{funcName}]</color> {msg}");
        }
        else
            UnityEngine.Debug.Log($"[{pluginName}.{className}.{funcName}] {msg}");

        if (EnableFileLog)
            WriteFileLog($"[V][{pluginName}.{className}.{funcName}] {msg}");
    }

    public static void Warning(string pluginName, string className, string funcName, string msg)
    {
        if ((int)ELogLevel.Warning < (int)LogLevel || EPlatformType.Unknown == PlatformType)
            return;

        if (EPlatformType.UnityEditor == PlatformType)
        {
            var color = GetColor(pluginName);
            UnityEngine.Debug.LogWarning($"<color={color}>[{pluginName}.{className}.{funcName}]</color> {msg}");
        }
        else
            UnityEngine.Debug.LogWarning($"[{pluginName}.{className}.{funcName}] {msg}");

        if (EnableFileLog)
            WriteFileLog($"[W][{pluginName}.{className}.{funcName}] {msg}");
    }

    public static void Error(string pluginName, string className, string funcName, string msg)
    {
        if (EPlatformType.Unknown == PlatformType)
            return;

        if (ErrorToServerOnly || (int)ELogLevel.Error < (int)LogLevel)
        {
            _onSendErrorToServer?.Invoke($"[{pluginName}.{className}.{funcName}] {msg}");
            return;
        }

        if (EPlatformType.UnityEditor == PlatformType)
        {
            var color = GetColor(pluginName);
            UnityEngine.Debug.LogError($"<color={color}>[{pluginName}.{className}.{funcName}]</color> {msg}");
        }
        else
            UnityEngine.Debug.LogError($"[{pluginName}.{className}.{funcName}] {msg}");

        if (EnableFileLog)
            WriteFileLog($"[E][{pluginName}.{className}.{funcName}] {msg}");
    }

    public static void SetOnSendErrorToServer(Action<string> onSendErrorToServer)
    {
        _onSendErrorToServer = onSendErrorToServer;
    }

    private static string GetColor(string pluginName)
    {
        if (_colors.TryGetValue(pluginName, out var color))
            return color;

        return _defaultColor;
    }

    private static void WriteFileLog(string msg)
    {
#if UNITY_EDITOR
        if (!StzPluginEditorCore.StzLogConnector.Instance.Init())
            return;
        StzPluginEditorCore.StzLogConnector.Instance.UseFileLog = true;
#endif

        if (!File.Exists(FILE_LOG_PATH))
            File.Create(FILE_LOG_PATH);

        File.AppendAllText(FILE_LOG_PATH, msg + Environment.NewLine);
    }
}