using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class STZq
{
    
    static object lockObj = new object();
    static Queue<String> queue = new Queue<String>();
    private static bool flag = false;
    public static void Pop()
    {
        lock (lockObj)
        {
            flag = true;
            string temp_string = queue.Dequeue().ToString();
            StzPluginLogger.Verbose("STZq", "STZq", temp_string + "Listening", "Start");
            if (SundaytozNativeExtension.Instance.sendFunction(temp_string) == true)
            {
                StzPluginLogger.Verbose("STZq", "STZq", temp_string + "Listening", "Succeeded");
                FuncResponse.Instance.sendFunction(temp_string);
            }

            flag = false;
        }
        tryPop();
    }
    public static void Push(string item)
    {
        lock (lockObj)
        {
            queue.Enqueue(item);
        }
        tryPop();
    }

    public static int Count
    {
        get
        {
            lock (lockObj)
            {
                return queue.Count;
            }
        }
    }
 
    public static String Get()
    {
        lock (lockObj)
        {
            return queue.Peek();
        }
    }
 
    public static void Clear()
    {
        lock (lockObj)
        {
            queue.Clear();
        }
    }

    public static void tryPop()
    {
        if (flag!=true&&queue.Count!=0)
        {
            Pop();
        }
    }

    public static void Dispose()
    {
    }
}