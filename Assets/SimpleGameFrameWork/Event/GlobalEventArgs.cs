using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalEventArgs : EventArgs,IReference
{
    public abstract void Clear();
    public abstract int Id { get; }
}
