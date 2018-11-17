using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FsmManager : ManagerBase
{
    private Dictionary<string, IFsm> m_Fsms;
    private List<IFsm> m_TempFsms;

    public override int Priority
    {
        get { return 60; }
    }

    public override void Init()
    {
        m_Fsms=new Dictionary<string, IFsm>();
        m_TempFsms=new List<IFsm>();
    }

    public override void Shutdown()
    {
        foreach (KeyValuePair<string ,IFsm> fsm in m_Fsms)
        {
            fsm.Value.Shutdown();
        }
        m_Fsms.Clear();
        m_TempFsms.Clear();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        m_TempFsms.Clear();
        if (m_Fsms.Count <= 0)
        {
            return;
        }

        foreach (KeyValuePair<string, IFsm> fsm in m_Fsms)
        {
            m_TempFsms.Add(fsm.Value);
        }

        foreach (IFsm fsm in m_TempFsms)
        {
            if (fsm.IsDestroyed)
            {
                continue;
            }
            fsm.Update(elapseSeconds,realElapseSeconds);
        }
    }
    /// <summary>
    /// Whether there is a StatemMachine
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>
    private bool HasFsm(string fullName)
    {
        return m_Fsms.ContainsKey(fullName);
    }
    public bool HasFsm<T>()
    {
        return HasFsm(typeof(T));
    }

    public bool HasFsm(Type type)
    {
        return HasFsm(type.FullName);
    }
    public Fsm<T> CreateFsm<T>(T owner, string name = "", params FsmState<T>[] states) where T : class
    {
        if (HasFsm<T>())
        {
            Debug.LogError("The StateMachine to be created already exists");
        }
        if (name == "")
        {
            name = typeof(T).FullName;
        }
        Fsm<T> fsm=new Fsm<T>(name,owner,states);
        m_Fsms.Add(name,fsm);
        return fsm;
    }

    public bool DestroyFsm(string name)
    {
        IFsm fsm = null;
        if (m_Fsms.TryGetValue(name, out fsm))
        {
            fsm.Shutdown();
            return m_Fsms.Remove(name);
        }

        return false;
    }

    public bool DestroyFsm<T>() where T : class
    {
        return DestroyFsm(typeof(T).FullName);
    }

    public bool DestroyFsm(IFsm fsm)
    {
        return DestroyFsm(fsm.Name);
    }

}
