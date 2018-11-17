using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Fsm<T> : IFsm where T : class
{
    public string Name { get; private set; }
    public Type OwnerType
    {
        get { return typeof(T); }
    }

    public bool IsDestroyed { get; private set; }
    public float CurrentStateTime { get; private set; }
    /// <summary>
    /// state dictionary for stateMachine
    /// </summary>
    private Dictionary<string, FsmState<T>> m_States;

    private Dictionary<int, FsmState<T>> mId_States;
    /// <summary>
    /// Data dictionary for stateMachine
    /// </summary>
    private Dictionary<string, object> m_Datas;
    public FsmState<T> CurrentState { get; private set; }
    /// <summary>
    /// stateMachine Onwer
    /// </summary>
    public T Owner { get; private set; }

    public void Shutdown()
    {
        if (CurrentState!=null)
        {
            CurrentState.OnLeave(this,true);
            CurrentState = null;
            CurrentStateTime = 0f;
        }

        foreach (KeyValuePair<string,FsmState<T>> state in m_States)
        {
            state.Value.OnDestroy(this);
        }
        m_States.Clear();
        m_Datas.Clear();
        IsDestroyed = true;
    }
    public void Update(float elapseSeconds, float realElapseSeconds)
    {
        if (CurrentState==null)
        {
            return;
        }

        CurrentStateTime += elapseSeconds;
        CurrentState.OnUpdate(this,elapseSeconds,realElapseSeconds);
    }

    public Fsm()
    {
        mId_States=new Dictionary<int, FsmState<T>>();
    }

    public Fsm(string name,T owner,params FsmState<T>[] states)
    {     
        if (owner==null)
        {
            Debug.LogError("StateMachine Owner is null");
        }

        if (states == null || states.Length < 1)
        {
            Debug.LogError("There is no state to add to the StatemMachine");
        }

        Name = name;
        Owner = owner;
        m_States = new Dictionary<string, FsmState<T>>();
        m_Datas=new Dictionary<string, object>();
        foreach (FsmState<T> state in states)
        {
            if (state==null)
            {
                Debug.LogError("The state to be added to the StatemMachine is empty");
            }

            string stateName = state.GetType().FullName;
            if (m_States.ContainsKey(stateName))
            {
                Debug.LogError("The state to be added to the StatemMachine already exists"+stateName);
            }
            m_States.Add(stateName,state);
            if (state.stateID!=-1)
            {
                mId_States.Add(state.stateID, state);
            }

            state.OnInit(this);
        }

        CurrentStateTime = 0f;
        CurrentState = null;
        IsDestroyed = false;
    }
    public void AddState<TState>(FsmState<T> state) where TState : FsmState<T>
    {
        string stateName = state.GetType().FullName;
        if (m_States.ContainsKey(stateName))
        {
            Debug.LogError("The state to be added to the StatemMachine already exists" + stateName);
        }
        m_States.Add(stateName, state);
    }
    public void AddIDState(FsmState<T> state)
    {
        if (!mId_States.ContainsKey(state.stateID))
        {
            mId_States.Add(state.stateID, state);
        }
    }
    public TState GetState<TState>() where TState : FsmState<T>
    {
        return GetState(typeof(TState)) as TState;
    }
    public FsmState<T> GetState(Type stateType)
    {    
        if (stateType==null)
        {
            Debug.LogError("The status to get is empty");
        }

        if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
        {
            Debug.LogError("The status to get "+stateType.FullName+"No direct or indirect implementation"+typeof(FsmState<T>).FullName);
        }

        FsmState<T> state = null;
        if (m_States.TryGetValue(stateType.FullName,out state))
        {
            return state;
        }

        return null;
    }
    public FsmState<T> GetState(int  stateID)
    {
        if (stateID <0)
        {
            Debug.LogError("The status to get is empty");
        }
        FsmState<T> state = null;
        if (mId_States.TryGetValue(stateID, out state))
        {
            return state;
        }
        return null;
    }
    public void ChangeState(int stateID)
    {
        FsmState<T> state = null;
        if (mId_States.TryGetValue(stateID, out state))
        {
            CurrentState.OnLeave(this,false);
            CurrentState = state;
            CurrentState.OnEnter(this);
        }
    }
    public void ChangeState<TState>() where TState : FsmState<T>
    {
        ChangeState(typeof(TState));
    }

    public void ChangeState(Type type)
    {
        if (CurrentState==null)
        {
            Debug.LogError("The current StatemMachine state is empty and cannot be switched.");
        }

        FsmState<T> state = GetState(type);
        if (state==null)
        {
            Debug.Log("get state is empty and cannot be switched"+type.FullName);
        }
        CurrentState.OnLeave(this,false);
        CurrentStateTime = 0f;
        CurrentState = state;
        CurrentState.OnEnter(this);
    }

    public void Start<TState>() where TState : FsmState<T>
    {
        Start(typeof(TState));
    }

    public void Start(int StateId)
    {
        if (CurrentState != null)
        {
            Debug.LogError("CurrentState is Start ,cannot be start again");
        }

        if (StateId <0)
        {
            Debug.LogError("The status to start is empty and cannot start");
        }

        FsmState<T> state = GetState(StateId);
        if (state == null)
        {
            Debug.LogError("The get statue is empty and cannot start");
        }

        CurrentStateTime = 0f;
        CurrentState = state;
        CurrentState.OnEnter(this);
    }

    public void Start(Type stateType)
    {
        if (CurrentState!=null)
        {
            Debug.LogError("CurrentState is Start ,cannot be start again");
        }

        if (stateType==null)
        {
            Debug.LogError("The status to start is empty and cannot start");
        }

        FsmState<T> state = GetState(stateType);
        if (state==null)
        {
            Debug.LogError("The get statue is empty and cannot start");
        }

        CurrentStateTime = 0f;
        CurrentState = state;
        CurrentState.OnEnter(this);
    }
    /// <summary>
    /// Throw a state machine event
    /// </summary>
    /// <param name="sender">event source</param>
    /// <param name="eventId">eventId</param>
    public void FireEvent(object sender,int eventId)
    {
        if (CurrentState==null)
        {
            Debug.Log("current state is empty and cannot to throw event");
        }
        CurrentState.OnEvent(this,sender,eventId,null);
    }
    /// <summary>
    /// is have StateData
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool HasData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("The StatemMachine data name to be Queried is empty");
        }

        return m_Datas.ContainsKey(name);
    }
    /// <summary>
    /// Get StatemMachine data
    /// </summary>
    /// <typeparam name="TDate"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public TDate GetData<TDate>(string name)
    {
        return (TDate) GetData(name);
    }
    /// <summary>
    /// Get StatemMachine data
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("The StatemMachine name to be Queried is empty");
        }

        object data = null;
        m_Datas.TryGetValue(name, out data);
        return data;
    }

    public void SetData(string name ,object data)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("The StatemMachine data name to be Set is empty.");
        }

        m_Datas[name] = data;
    }

    public bool RemoveData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("The StatemMachine data name to be Remove is empty.");
        }

        return m_Datas.Remove(name);
    }   
}
