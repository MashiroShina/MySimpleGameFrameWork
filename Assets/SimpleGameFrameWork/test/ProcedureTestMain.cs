using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureTestMain : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    ProcedureManager procedureManager = FrameworkEntry.Instance.GetManager<ProcedureManager>();
        //Add Procedure Entrance
        Procedure_Start entranceProcedure=new Procedure_Start();
        procedureManager.AddProcedure(entranceProcedure);
	    procedureManager.SetEntranceProcedure(entranceProcedure);
        //Add another Procedure
        procedureManager.AddProcedure(new Procedure_Play());
	    procedureManager.AddProcedure(new Procedure_Over());

        procedureManager.CreateProceduresFsm();
    }

}
