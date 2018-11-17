using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeManager : ManagerBase
{
    private static readonly string[] s_EmptyStringArray = new string[] { };
    public DataNode Root { get; private set; }
    private const string RootName = "<Root>";

    public DataNodeManager()
    {
        Root=new DataNode(RootName,null);
    }

    public override void Init()
    {
       // throw new System.NotImplementedException();
    }

    public override void Shutdown()
    {
        Root.Clear();
        Root = null;
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
       // throw new System.NotImplementedException();
    }

    /// <summary>
    /// Data node path segmentation
    /// </summary>
    private static string[] GetSplitPath(string path)
    {
        if (string .IsNullOrEmpty(path))
        {
            return s_EmptyStringArray;
        }

        return path.Split(DataNode.s_PathSplit, StringSplitOptions.RemoveEmptyEntries);
    }

    public DataNode GetNode(string path,DataNode node=null)
    {
        DataNode current = (node ?? Root);
        string[] splitPath = GetSplitPath(path);
        foreach (string ChildName in splitPath)
        {
            current = current.GetChild(ChildName);
            if (current==null)
            {
                return null;
            }
        }

        return current;
    }

    public DataNode GetOrAddNode(string path, DataNode node = null)
    {
        DataNode current = (node ?? Root);
        string[] splitPath = GetSplitPath(path);
        foreach (string childName in splitPath)
        {
            current = current.GetOrAddChild(childName);
        }

        return current;
    }
    public void RemoveNode(string path, DataNode node = null)
    {
        DataNode current = (node ?? Root);
        DataNode parent = current.Parent;
        string[] splitPath = GetSplitPath(path);
        foreach (string childName in splitPath)
        {
            if (parent==null)
            {
                parent = current;
            }
            current = current.GetChild(childName);
            if (current == null)
            {
                return;
            }
        }

        if (parent != null)
        {
            parent.RemoveChild(current.Name);
        }
    }
    public T GetData<T>(string path, DataNode node = null)
    {
        DataNode current = GetNode(path, node);
        if (current == null)
        {
            Debug.Log("要获取数据的结点不存在：" + path);
            return default(T);
        }

        return current.GetData<T>();

    }

    public void SetData(string path, object data, DataNode node = null)
    {
        DataNode current = GetOrAddNode(path, node);
        current.SetData(data);
    }


}
