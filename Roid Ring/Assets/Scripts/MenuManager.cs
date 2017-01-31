using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    private MyNetworkDiscovery nd;
	// Use this for initialization
	void Start () {
        nd = GetComponent<MyNetworkDiscovery>();
	}
    public void Exit()
    {
        Application.Quit();
    }

    public void Join()
    {
        nd.Initialize();
        nd.StartAsClient() ;
    }

    public void LoadGame()
    {
        NetworkManager.singleton.StartHost();
    }
}
