using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameworkEntry : ScriptSingleton<FrameworkEntry> {
    /// <summary>
    /// model manager for all
    /// </summary>
    public LinkedList<ManagerBase> m_Managers=new LinkedList<ManagerBase>();

    void Update()
    {
        //turn manage
        foreach (ManagerBase manager in m_Managers)
        {
            manager.Update(Time.deltaTime,Time.unscaledDeltaTime);
        }
    }

    void OnDestroy()
    {
        for (LinkedListNode<ManagerBase> current=m_Managers.Last; current !=null; current=current.Previous)
        {
            current.Value.Shutdown();
        }
    }
    /// <summary>
    /// Get appoint manageClass
    /// </summary>
    public T GetManager<T>() where T : ManagerBase
    {
        Type managerType = typeof(T);
        foreach (ManagerBase manager in m_Managers)
        {
            if (manager.GetType() == managerType)
            {
                return manager as T;
            }
        }

        //can't find goto create
        return CreateManager(managerType) as T;
    }
    /// <summary>
    /// Create manageClass
    /// </summary>
    /// <param name="managerType"></param>
    /// <returns></returns>
    private ManagerBase CreateManager(Type managerType)
    {
        ManagerBase manager = (ManagerBase) Activator.CreateInstance(managerType);
        if (manager==null)
        {
            Debug.LogError("ManageClass Create Faile"+manager.GetType().FullName);
        }
        //Decide position for LinkNode by model Priority
        LinkedListNode<ManagerBase> current = m_Managers.First;
        while (current!=null)
        {
            if (manager.Priority>current.Value.Priority)
            {
                break;
            }

            current = current.Next;
        }

        if (current!=null)
        {
            m_Managers.AddBefore(current, manager);
        }
        else
        {
            m_Managers.AddLast(manager);
        }
        manager.Init();
        return manager;
    }
}
