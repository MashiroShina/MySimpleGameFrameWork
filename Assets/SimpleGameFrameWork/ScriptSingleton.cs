using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptSingleton<T>: MonoBehaviour where T:ScriptSingleton<T>
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance==null)
            {
                //Can for scenes Find T Script object
                _instance = FindObjectOfType<T>();
                if (FindObjectsOfType<T>().Length>1)
                {
                    Debug.LogError("Scene Singleton scripts count >1"+_instance.GetType().ToString());
                    return _instance;
                }
                //Can't for scenes Find T Script 
                if (_instance==null)
                {
                    string instanceName = typeof(T).Name;
                    GameObject instanceGo=GameObject.Find(instanceName);
                    if (instanceGo==null)
                    {
                        instanceGo=new GameObject(instanceName);
                        DontDestroyOnLoad(instanceGo);
                        _instance = instanceGo.AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                    else
                    {
                        //scenes have some identical object name print log
                        Debug.LogError("Scenes have object mount some identical singleton scripts ");
                    }
                }
            }

            return _instance;
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

}
