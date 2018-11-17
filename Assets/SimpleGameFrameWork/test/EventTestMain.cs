using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FrameworkEntry.Instance.GetManager<EventManager>().Subscribe(1,EventTestMethod);
	}
    private void EventTestMethod(object sender, GlobalEventArgs e)
    {
        EventTestArgs args = e as EventTestArgs;
        Debug.Log(args.m_Name);
    }

	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        EventTestArgs e = ReferencePool.Acquire<EventTestArgs>();

	        //派发事件
	        FrameworkEntry.Instance.GetManager<EventManager>().Fire(this, e.Fill("MyEventArgsFire"));
	    }
	}
}
