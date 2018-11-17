using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceCollection
{
    private Queue<IReference> m_References;

    public ReferenceCollection()
    {
        m_References=new Queue<IReference>();
    }
    /// <summary>
    /// Get quote
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Acquire<T>() where T : class, IReference, new()
    {
        lock (m_References)
        {
            if (m_References.Count>0)
            {   
                return m_References.Dequeue() as T;
            }
        }
        //Debug.Log(typeof(T).FullName.ToString() + "-NewT");
        return new T();
    }

    /// <summary>
    /// get quote
    /// </summary>
    /// <param name="referenceType"></param>
    /// <returns></returns>
    public IReference Acquire(Type referenceType)
    {
        lock (m_References)
        {
            if (m_References.Count > 0)
            {
                return m_References.Dequeue();
            }

            return (IReference) Activator.CreateInstance(referenceType);
        }
    }

    public void Release<T>(T reference) where T : class, IReference
    {
        reference.Clear();
        lock (m_References)
        {
            m_References.Enqueue(reference);
        }
    }

    public void Add<T>(int count) where T : class, IReference, new()
    {
        lock (m_References)
        {
            while (count-->0)
            {
                m_References.Enqueue(new T());
            }
        }
    }

    public void Remove<T>(int count)where T:class ,IReference
    {
        lock (m_References)
        {
            if (count>m_References.Count)
            {
                count = m_References.Count;
            }

            while (count-->0)
            {
                m_References.Dequeue();
            }
        }
    }

    public void RemoveAll()
    {
        lock (m_References)
        {
            m_References.Clear();
        }
    }

}
