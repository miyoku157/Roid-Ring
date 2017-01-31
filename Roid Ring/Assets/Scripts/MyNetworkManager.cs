using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class MyNetworkManager : NetworkManager {
    public int playerMax = 2;
    public MyNetworkDiscovery discovery;
    // Use this for initialization
    void Start () {
        discovery = GetComponent<MyNetworkDiscovery>();
    }

    public override void OnStartHost()
    {
        discovery.Initialize();
        discovery.StartAsServer();

    }
    public override void OnStopClient()
    {
        discovery.StopBroadcast();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (base.numPlayers > playerMax)
        {
            discovery.StopBroadcast();
        }
    }
}
