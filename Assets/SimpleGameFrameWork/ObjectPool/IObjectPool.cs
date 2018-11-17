using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool  {
    string Name { get; }
    Type ObjecType { get; }
    /// <summary>
    /// object in objectpool count
    /// </summary>
    int Count { get; }
    int CanReleaseCount { get; }
    /// <summary>
    /// objectPool AutoReleaseInterval seconds(An automatic release in seconds.)
    /// </summary>
    float AutoReleaseInterval { get; set; }
    /// <summary>
    /// object capacity
    /// </summary>
    int Capacity { get; set; }
    /// <summary>
    /// The number of expired seconds of object pool object
    /// (recovered for a few seconds is deemed to be out of date and needs to be released).
    /// </summary>
    float ExpireTime { get; set; }
    /// <summary>
    /// Release the releasable object beyond the pool capacity of the object.
    /// </summary>
    void Release();
    /// <summary>
    /// Release specified number of releasable objects
    /// </summary>
    /// <param name="toReleaseCount">Try to release object count</param>
    void Release(int toReleaseCount);
    /// <summary>
    /// Release Unused object for All;
    /// </summary>
    void ReleaseAllUnused();

    void Update(float elapseSeconds,float realElapseSeconds);
    void Shutdown();
}
