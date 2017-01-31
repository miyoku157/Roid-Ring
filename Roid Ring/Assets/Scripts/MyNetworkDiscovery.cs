using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class MyNetworkDiscovery : NetworkDiscovery {

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        MyNetworkManager nm=(MyNetworkManager)NetworkManager.singleton;
        nm.networkAddress = fromAddress;
        nm.StartClient();
        nm.discovery.StopBroadcast();
    }
}
