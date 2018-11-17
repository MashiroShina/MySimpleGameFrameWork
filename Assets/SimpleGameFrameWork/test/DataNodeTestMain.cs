using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeTestMain : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    DataNodeManager dataNodeManager = FrameworkEntry.Instance.GetManager<DataNodeManager>();
        dataNodeManager.SetData("Player.Name","Ellan");
	    string playerName = dataNodeManager.GetData<string>("Player.Name");
	    Debug.Log(playerName);

	    DataNode playerNode = dataNodeManager.GetNode("Player");
        dataNodeManager.SetData("Level",99,playerNode);
	    int playerLevel = dataNodeManager.GetData<int>("Level", playerNode);
	    int playerLeve2 = dataNodeManager.GetData<int>("Player.Level");
	    Debug.Log(playerLevel+ playerLeve2.ToString());

	    DataNode playerExpNode = playerNode.GetOrAddChild("Exp");
	    playerExpNode.SetData(1000);
	    int playerExp = playerExpNode.GetData<int>();
	    Debug.Log(playerExp);

	  
    }

    // Update is called once per frame
    void Update () {
		
	}
}
