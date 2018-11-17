using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTestMain : MonoBehaviour
{
    private ObjectPool<TestObject> m_testPool;

    private ObjectPoolManager m_objectPoolManager;
    // Use this for initialization
    void Start ()
	{
	    m_objectPoolManager = FrameworkEntry.Instance.GetManager<ObjectPoolManager>();
	    m_testPool = m_objectPoolManager.CreateObjectPool<TestObject>(1,10);
        TestObject testObject = new TestObject("hello", "Test1");
	    TestObject testObject2 = new TestObject("Object", "Test2");
        m_testPool.Regiser(testObject,false);
	    m_testPool.Regiser(testObject2, false);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        TestObject testObject = m_testPool.Spawn("Test1");
	        TestObject testObject2 = m_testPool.Spawn("Test2");
            m_testPool.Unspawn(testObject.Target);
            m_testPool.Unspawn(testObject2.Target);
            Debug.Log(testObject.Target+" "+testObject2.Target+" "+m_testPool.CanReleaseCount);
	    }
	}
}
