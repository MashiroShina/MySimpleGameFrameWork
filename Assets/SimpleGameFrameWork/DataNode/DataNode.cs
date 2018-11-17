using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNode  {
    public static readonly DataNode[] s_EmptyArray=new DataNode[]{};
    public static readonly string []s_PathSplit=new string[]{ ".", "/", "\\" };
    /// <summary>
    /// node Name
    /// </summary>
    public string Name { get; private set; }
    public DataNode Parent { get; private set; }
    public string FullName
    {
        get { return Parent == null ? Name : string.Format("{0}{1}{2}", Parent.FullName, s_PathSplit[0], Name); }
    }
    /// <summary>
    /// Node Data
    /// </summary>
    private object m_Data;

    private List<DataNode> m_Childs;

    public int ChildCount
    {
        get { return m_Childs!=null?m_Childs.Count:0;}
    }

    private static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        foreach (string pathSplit in s_PathSplit)
        {
            if (name.Contains(pathSplit))
            {
                return false;
            }
        }

        return true;
    }

    public DataNode(string name, DataNode parent)
    {
        if (!IsValidName(name))
        {
            Debug.LogError("DataNode name is UnValideName"+name);
        }

        Name = name;
        m_Data = null;
        Parent = parent;
        m_Childs = null;
    }
    public T GetData<T>()
    {
        return (T) m_Data;
    }

    public void SetData(object data)
    {
        m_Data = data;
    }
    /// <summary>
    /// Get sub-data nodes based on index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public DataNode GetChild(int index)
    {
        return index >= ChildCount ? null : m_Childs[index];
    }

    public DataNode GetChild(string name)
    {
        if (!IsValidName(name))
        {
            Debug.LogError("Sub node is UnValid ,and can't get it");
            return null;
        }

        if (m_Childs == null)
        {
            return null;
        }

        foreach (DataNode child in m_Childs)
        {
            if (child.Name==name)
            {
                return child;
            }
        }

        return null;
    }

    public DataNode GetOrAddChild(string name)
    {
        DataNode node = GetChild(name);//==new child
        if (node!=null)
        {
            return node;
        }
        node=new DataNode(name,this);// there this is father class/node
        if (m_Childs==null)//this m_Child ==node parent ，== parent add child
        {
            m_Childs=new List<DataNode>();
        }
        m_Childs.Add(node);
        return node;
    }

    public void RemoveChild(int index)
    {
        DataNode node = GetChild(index);
        if (node==null)
        {
            return;
        }

        node.Clear();
        m_Childs.Remove(node);
    }
    public void RemoveChild(string name)
    {
        DataNode node = GetChild(name);
        if (node == null)
        {
            return;
        }

        node.Clear();
        m_Childs.Remove(node);
    }
    public void Clear()
    {
        m_Data = null;
        if (m_Childs!=null)
        {
            foreach (DataNode child in m_Childs)
            {
                child.Clear();
            }
            m_Childs.Clear();
        }
    }
}

