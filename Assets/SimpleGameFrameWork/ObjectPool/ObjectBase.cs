using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase
{

    public string Name { get; set; }
    /// <summary>
    /// object(The objects actually used are placed here.)
    /// </summary>
    public object Target { get; set; }
    /// <summary>
    /// Object Last usetime
    /// </summary>
    public DateTime LastUseTime { get;private set; }
    /// <summary>
    /// Object acquisition count
    /// </summary>
    public int SpawnCount { get; set; }
    /// <summary>
    /// Whether the object is in use
    /// </summary>
    public bool IsInUse
    {
        get { return SpawnCount > 0; }
    }

    public ObjectBase(object target,string name)
    {
        Name = name;
        Target = target;
    }
    /// <summary>
    /// When getting an object
    /// </summary>
    protected virtual void OnSpawn()
    {
    }
    /// <summary>
    /// When recycling objects
    /// </summary>
    protected virtual void OnUnSpawn()
    {
    }
    /// <summary>
    /// when release objects
    /// </summary>
    public abstract void Release();
    /// <summary>
    /// Getting objects
    /// </summary>
    /// <returns></returns>
    public ObjectBase Spawn()
    {
        SpawnCount++;
        LastUseTime=DateTime.Now;
        OnSpawn();
        return this;
    }
    /// <summary>
    /// Recycling an object
    /// </summary>
    public void Unspawn()
    {
        OnUnSpawn();
        LastUseTime=DateTime.Now;
        SpawnCount--;
    }
}
