using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferencePool  {

	private static Dictionary<string ,ReferenceCollection>s_ReferenceCollections=new Dictionary<string, ReferenceCollection>();

    public static int Count
    {
        get { return s_ReferenceCollections.Count; }
    }
    /// <summary>
    /// Get and Add ReferenceCollection
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public static ReferenceCollection GetReferenceCollection(string fullName)
    {
        ReferenceCollection referenceCollection = null;
        lock (s_ReferenceCollections)
        {
            if (!s_ReferenceCollections.TryGetValue(fullName,out referenceCollection))
            {
                referenceCollection=new ReferenceCollection();
                s_ReferenceCollections.Add(fullName,referenceCollection);
               // Debug.Log("addsSome Name-" + fullName);
            }
        }

        return referenceCollection;
    }

    public static void ClearAll()
    {
        lock (s_ReferenceCollections)
        {
            foreach (KeyValuePair<string,ReferenceCollection> referenceCollection in s_ReferenceCollections)
            {
                referenceCollection.Value.RemoveAll();
            }
            s_ReferenceCollections.Clear();
        }
    }
    /// <summary>
    /// Add specification in quantities collection
    /// </summary>
    /// <typeparam name="T">quota Type</typeparam>
    /// <param name="count">Add count</param>
    public static void Add<T>(int count) where T : class, IReference, new()
    {
        GetReferenceCollection(typeof(T).FullName).Add<T>(count);
    }
    /// <summary>
    /// Removal specification in quantities collection
    /// </summary>
    /// <typeparam name="T">quota Type</typeparam>
    /// <param name="count">Add count</param>
    public static void Remove<T>(int count) where T : class, IReference, new()
    {
        GetReferenceCollection(typeof(T).FullName).Remove<T>(count);
    }
    public static void RemoveAll<T>() where T : class, IReference
    {
        GetReferenceCollection(typeof(T).FullName).RemoveAll();
    }

    public static T Acquire<T>() where T : class, IReference,new ()
    {
       // Debug.Log(typeof(T).FullName + "-AcquireT.fullName");
        return GetReferenceCollection(typeof(T).FullName).Acquire<T>();
    }

    public static IReference Acquire(Type referenceType)
    {
        return GetReferenceCollection(referenceType.FullName).Acquire(referenceType);
    }
    /// <summary>
    /// Foreign quotation ref quoting set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reference"></param>
    public static void Release<T>(T reference) where T : class, IReference
    {
        //Debug.Log(reference.ToString() + "-Release");
        if (reference==null)
        {
            Debug.LogError("Essential refund quotation");
        }
        GetReferenceCollection(typeof(T).FullName).Release(reference);
    }
}
