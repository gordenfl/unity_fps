using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using System;

public class Menu : GlobalEventListener
{
    public Button loginButton;
    public Button startServer;
    public int ServerPort = 8099;
    // Start is called before the first frame update
    void Start()
    {
    }


    public void OnmyStartServer()
    {
        BoltLauncher.StartServer(this.ServerPort);
    }
    public void OnLoginButtonClick()
    {
        BoltLauncher.StartClient();
    }

    public override void BoltStartDone()
    {
        if(BoltNetwork.IsServer)
        {
            string servername = System.Guid.NewGuid().ToString();
            BoltMatchmaking.CreateSession(
                sessionID: servername,
                sceneToLoad: "Battle");//here will load scene
        }
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

    public override void SessionConnected(UdpSession session, IProtocolToken token)
    {
        Console.WriteLine("Sexxion Connection");
    }
}
