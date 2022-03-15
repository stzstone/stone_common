using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IPoolable
{
    void Dispose();
}
 
public class STZq<String>: IPoolable
{
    
    object lockObj = new object();
    Queue<String> queue = new Queue<String>(); 
    
    public void Pop()
    {
        
        lock (lockObj)
        {
            string temp_string = queue.Dequeue().ToString();
            SundaytozNativeExtension.Instance.sendFunction(temp_string);
           
            
        }
    }
    public void Push(String item)
    {
        lock (lockObj)
        {
            queue.Enqueue(item);
        }
        this.Pop();
    }

    public int Count
    {
        get
        {
            lock (lockObj)
            {
                return queue.Count;
            }
        }
    }
 
    public String Get()
    {
        lock (lockObj)
        {
            return queue.Peek();
        }
    }
 
    public void Clear()
    {
        lock (lockObj)
        {
            queue.Clear();
        }
    }
 
    public void Dispose()
    {
    }
}