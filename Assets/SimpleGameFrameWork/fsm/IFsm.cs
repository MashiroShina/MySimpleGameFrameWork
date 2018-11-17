using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFsm
{
    /// <summary>
    /// Fsm Name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Fsm Holder
    /// </summary>
    Type OwnerType { get; }
    /// <summary>
    /// Whether Fsm is destroyed
    /// </summary>
    bool IsDestroyed { get; }
    /// <summary>
    /// Currentstate running Time
    /// </summary>
    float CurrentStateTime { get; }
    /// <summary>
    /// Fsm Update
    /// </summary>
    /// <param name="elapseSeconds"></param>
    /// <param name="realElapseSeconds"></param>
    void Update(float elapseSeconds, float realElapseSeconds);
    /// <summary>
    /// close && clear
    /// </summary>
    void Shutdown();
}
