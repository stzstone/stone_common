using System;
using System.Collections;
using System.Collections.Generic;
public interface IPoolable
{
    void Dispose();
}
 
public class STZq<Action> : IPoolable
{
    object lockObj = new object();
    Queue<Action> queue = new Queue<Action>();
 
    public void Push(Action item)
    {
        lock (lockObj)
        {
            queue.Enqueue(item);
        }
    }
 
    public void Pop()
    {
        lock (lockObj)
        {
            Action action = queue.Dequeue();
            SundaytozNativeExtension.doAction(action);
        }
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
 
    public Action Get()
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