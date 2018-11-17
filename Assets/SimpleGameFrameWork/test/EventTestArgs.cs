using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestArgs : GlobalEventArgs {
    public override int Id
    {
        get { return 1; }
    }

    public override void Clear()
    {
        m_Name = string.Empty;
    }

    public string m_Name;

    public EventTestArgs Fill(string name)
    {
        this.m_Name = name;
        return this;
    }
}
