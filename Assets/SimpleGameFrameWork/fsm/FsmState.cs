using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Response method template for state machine events
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="fsm"></param>
/// <param name="sender"></param>
/// <param name="userData"></param>
public delegate void FsmEventHandler<T>(Fsm<T> fsm, object sender, object userData) where T : class;
public class FsmState<T> where T: class
{
    public int stateID=-1;  
    /// <summary>
    /// State Subscirbe Dictionary
    /// </summary>
    private Dictionary<int, FsmEventHandler<T>> m_EventHandlers;
    public FsmState()
    {
        m_EventHandlers=new Dictionary<int, FsmEventHandler<T>>();  
    }
    protected void SubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
    {
        if (eventHandler==null)
        {
            Debug.LogError("State Event Is null ,Can't Subscribe StateEvent");
        }

        if (!m_EventHandlers.ContainsKey(eventId))
        {
            m_EventHandlers[eventId] = eventHandler;
        }
        else
        {
            m_EventHandlers[eventId] += eventHandler;
        }
    }

    protected void UnsubscribeEvent(int eventId,FsmEventHandler<T> eventHandler)
    {
        if (eventHandler == null) 
        {
            Debug.LogError("State Event Is null ,Can't UnSubscribe StateEvent");
        }

        if (m_EventHandlers.ContainsKey(eventId))
        {
            m_EventHandlers[eventId] -= eventHandler;
        }
    }

    private void SubscribeEventTest(Fsm<T> fsm, object sender, object userData)
    {
        SubscribeEvent(1, SubscribeEventTest);
    }
    public void OnEvent(Fsm<T> fsm,object sender,int eventId,object userData)
    {  
        FsmEventHandler<T> eventHandlers = null;
        if (m_EventHandlers.TryGetValue(eventId,out eventHandlers))
        {
            if (eventHandlers!=null)
            {
                eventHandlers(fsm, sender, userData);
            }
        }
    }
    /// <summary>
    /// FsmState Init
    /// </summary>
    /// <param name="fsm">FsmState quote</param>
    public virtual void OnInit(Fsm<T> fsm)
    {

    }

    /// <summary>
    /// FsmState Enter transfer
    /// </summary>
    /// <param name="fsm">FsmState quote</param>
    public virtual void OnEnter(Fsm<T> fsm)
    {

    }

    /// <summary>
    /// FsmState Update transfer
    /// </summary>
    /// <param name="fsm">FsmState quote</param>
    public virtual void OnUpdate(Fsm<T> fsm, float elapseSeconds, float realElapseSeconds)
    {

    }

    /// <summary>
    /// FsmState Leave transfer
    /// </summary>
    /// <param name="fsm">FsmState quote</param>
    /// <param name="isShutdown">close state trigger</param>
    public virtual void OnLeave(Fsm<T> fsm, bool isShutdown)
    {

    }

    /// <summary>
    /// FsmState Destroy transfer
    /// </summary>
    /// <param name="fsm">FsmState quote</param>
    public virtual void OnDestroy(Fsm<T> fsm)
    {
        m_EventHandlers.Clear();
    }
    protected void ChangeState<TState>(Fsm<T> fsm) where TState : FsmState<T>
    {
        ChangeState(fsm, typeof(TState));
    }

    protected void ChangeState(Fsm<T> fsm, Type type)
    {
        if (fsm==null)
        {
            Debug.Log("The StatemMachine that needs to switch state is empty and cannot be switched");
        }

        if (type==null)
        {
            Debug.Log("The StatemMachine that needs to switch state is empty and cannot be switched");
        }

        if (!typeof(FsmState<T>).IsAssignableFrom(type))
        {
            Debug.Log("The state to be switched does not directly or indirectly implement FsmState<T>, cannot be switched");
        }

        fsm.ChangeState(type);
    }

}

