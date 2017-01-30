using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class MyNetworkDiscovery : NetworkDiscovery {

	// Use this for initialization
	void Start () {
		
	}

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();       
        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
    }
}
