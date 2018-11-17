using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureBase : FsmState<ProcedureManager>
{
    public override void OnEnter(Fsm<ProcedureManager> fsm)
    {
        base.OnEnter(fsm);
        Debug.Log("Enter ProcedureBase"+GetType().FullName);
    }

    public override void OnLeave(Fsm<ProcedureManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        Debug.Log("Leave ProcedureBase" + GetType().FullName);
    }
}
