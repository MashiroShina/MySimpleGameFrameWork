using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase  {
    /// <summary>
    /// model priority ,high priority can be first use and close
    /// </summary>
    public virtual int Priority
    {
        get
        {
            return 0;
        }
    }
    /// <summary>
    /// set Init model
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// turn Update model
    /// </summary>
    /// <param name="elapseSeconds">logic seconds</param>
    /// <param name="realElapseSeconds">realy seconds</param>
    public abstract void Update(float elapseSeconds, float realElapseSeconds);
    /// <summary>
    /// close and clear model
    /// </summary>
    public abstract void Shutdown();

}
