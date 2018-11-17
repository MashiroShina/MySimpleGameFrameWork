using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPool<T> where T : GlobalEventArgs
{

    private class Event
    {
        public Event(object sender, T e)
        {
            Sender = sender;
            EventArgs = e;
        }

        public object Sender { get; private set; }
        public T EventArgs { get; private set; }
    }

    /// <summary>
    /// event And Handle method dictionary
    /// </summary>
    private Dictionary<int, EventHandler<T>> m_EventHandlers;

    /// <summary>
    /// event node queue
    /// </summary>
    private Queue<Event> m_Events;

    public EventPool()
    {
        m_EventHandlers=new Dictionary<int, EventHandler<T>>();
        m_Events=new Queue<Event>();
    }

    public bool Check(int id, EventHandler<T> handler)
    {
        if (handler==null)
        {
            Debug.LogError("Event handle is null");
            return false;
        }

        EventHandler<T> handlers = null;
        if (!m_EventHandlers.TryGetValue(id,out handlers))
        {
            return false;
        }

        if (handlers==null)
        {
            return false;
        }

        foreach (EventHandler<T> i in handlers.GetInvocationList())
        {
            if (i==handler)
            {
                return true;
            }
        }

        return false;
    }

    public void Subscribe(int id, EventHandler<T> handler)
    {
        if (handler == null)
        {
            Debug.LogError("Event handle is null and can't subscribe");
            return;
        }

        EventHandler<T> eventHandler = null;
        if (!m_EventHandlers.TryGetValue(id, out eventHandler)||eventHandler==null)
        {
            m_EventHandlers[id] = handler;
        }else if (Check(id, handler))
        {
            Debug.LogError("Will subscribe event handler is exist");
        }
        else
        {
            eventHandler += handler;
            m_EventHandlers[id] = eventHandler;
        }
    }

    public void Unsubscribe(int id, EventHandler<T> handler)
    {
        if (handler==null)
        {
            Debug.LogError("handler is null and can't Unsubscribe");
            return;
        }

        if (m_EventHandlers.ContainsKey(id))
        {
            m_EventHandlers[id] -= handler;
        }
    }

    private void HandleEvent(object sender ,T e)
    {
        int eventId = e.Id;
        EventHandler<T> handlers = null;
        if (m_EventHandlers.TryGetValue(eventId,out handlers))
        {
            if (handlers!=null)
            {
                handlers(sender, e);
            }
            else
            {
                Debug.LogError("Event don't have handle method"+eventId);
            }
        }
        ReferencePool.Release(e);
    }

    public void Update(float elapseSeconds, float realElapseSeconds)
    {
        while (m_Events.Count>0)
        {
            Event e = null;
            lock (m_Events)
            {
                e = m_Events.Dequeue();
            }
            HandleEvent(e.Sender,e.EventArgs);
        }
    }

    public void Fire(object sender, T e)
    {
        Event eventNode = new Event(sender, e);
        lock (m_Events)
        {
            m_Events.Enqueue(eventNode);
        }
    }

    public void FireNow(object sender,T e)
    {
        HandleEvent(sender,e);
    }

    public void Clear()
    {
        lock (m_Events)
        {
            m_Events.Clear();
        }
    }

    public void Shutdown()
    {
        Clear();
        m_EventHandlers.Clear();
    }

}
