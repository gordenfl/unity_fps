using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using System;

public class Menu : GlobalEventListener
{
    public Button loginButton;
    public int ServerPort = 8099;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoginButtonClick()
    {
        if(!BoltNetwork.IsServer)
        {
            BoltLauncher.StartServer(this.ServerPort);
        }
    }

    public override void BoltStartDone()
    {
        if(BoltNetwork.IsServer)
        {
            string servername = System.Guid.NewGuid().ToString();
            BoltMatchmaking.CreateSession(
                sessionID: servername,
                sceneToLoad: "World-1");
        }
        BoltLauncher.StartClient();
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach(var session in sessionList)
        {
            UdpSession s = session.Value;
            if(s.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(s);
            }
        }
    }
}
