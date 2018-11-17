using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : ObjectBase
{
    public TestObject(object target, string name) : base(target, name)
    {
       
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        Debug.Log(SpawnCount+Name+" : OnSpawnCount");
    }

    protected override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Debug.Log(SpawnCount+ Name + " : UnSpawnCount");
    }

    public override void Release()
    {
    }


}
