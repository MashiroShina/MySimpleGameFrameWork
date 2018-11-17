using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureManager : ManagerBase
{
    private FsmManager m_FsmManager;
    private Fsm<ProcedureManager> m_ProcedeureFsm;
    private List<ProcedureBase> m_procedures;
    /// <summary>
    /// Entry Procedure
    /// </summary>
    private ProcedureBase m_EntranceProcedure;

    public ProcedureBase CurrentProcedure
    {
        get
        {
            if (m_ProcedeureFsm==null)
            {
                Debug.LogError("The process StateMachine is empty and cannot get the current process");
            }

            return (ProcedureBase) m_ProcedeureFsm.CurrentState;
        }
    }

    public override int Priority
    {
        get { return -10; }
    }

    public ProcedureManager ()
    {
        m_FsmManager = FrameworkEntry.Instance.GetManager<FsmManager>();
        m_ProcedeureFsm = null;
        m_procedures=new List<ProcedureBase>();
    }

    public override void Init()
    {
       
    }

    public override void Shutdown()
    {
        
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        
    }

    public void AddProcedure(ProcedureBase procedure)
    {
        if (procedure==null)
        {
            Debug.Log("The process to be added is empty");
            return;
        }
        m_procedures.Add(procedure);
    }

    public void SetEntranceProcedure(ProcedureBase procedure)
    {
        m_EntranceProcedure = procedure;
    }

    public void CreateProceduresFsm()
    {
        m_ProcedeureFsm = m_FsmManager.CreateFsm(this, "", m_procedures.ToArray());
        if (m_EntranceProcedure==null)
        {
            Debug.LogError("The entry process is empty and cannot start the process.");
        }
        m_ProcedeureFsm.Start(m_EntranceProcedure.GetType());
    }
}
