using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        if (base.numPlayers > playerMax - 2)
        {
            Time.timeScale = 1;
            GameObject.Find("Canvas").GetComponentInChildren<Image>().enabled = false;
            discovery.StopBroadcast();
        }
        else
        {
            GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
            GameObject.Find("Canvas").GetComponentInChildren<Image>().enabled = true;
            Time.timeScale = 0;
            

        }

    }
    /*[Command]
    private void Cmdfreeze()
    {
        Time.timeScale = 0.1f;

    }
    [Command]
    private void CmdLaunch()
    {
        Time.timeScale = 0.1f;

    }*/
}
