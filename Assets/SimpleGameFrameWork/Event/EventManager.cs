using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ManagerBase
{
    private EventPool<GlobalEventArgs> m_EventPool;

    public override int Priority
    {
        get { return 100; }
    }

    public EventManager()
    {
        m_EventPool=new EventPool<GlobalEventArgs>();
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
    }

    public override void Shutdown()
    {
        //throw new System.NotImplementedException();
        m_EventPool.Shutdown();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        //throw new System.NotImplementedException();
        m_EventPool.Update(elapseSeconds, realElapseSeconds);
    }

    public bool Check(int id, EventHandler<GlobalEventArgs> handler)
    {
        return m_EventPool.Check(id, handler);
    }
    public void Subscribe(int id, EventHandler<GlobalEventArgs> handler)
    {
        m_EventPool.Subscribe(id, handler);
    }

    public void Unsubscribe(int id, EventHandler<GlobalEventArgs> handler)
    {
        m_EventPool.Unsubscribe(id, handler);
    }

    public void Fire(object sender, GlobalEventArgs e)
    {
        m_EventPool.Fire(sender, e);
    }

    public void FireNow(object sender, GlobalEventArgs e)
    {
        m_EventPool.FireNow(sender, e);
    }


}
