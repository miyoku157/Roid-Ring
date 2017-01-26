﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    public int playerMax = 2;
    private MyNetworkDiscovery discovery;

    private void Start()
    {
        discovery = GetComponent<MyNetworkDiscovery>();
    }

    public override void OnStartHost()
    {
        discovery.Initialize();
        discovery.StartAsServer();

    }

    public override void OnStartClient(NetworkClient client)
    {
        discovery.showGUI = false;
    }

    public override void OnStopClient()
    {
        discovery.StopBroadcast();
        discovery.showGUI = true;
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