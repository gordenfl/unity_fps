using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Bolt;

[BoltGlobalBehaviour]
public class CreatePlayer : GlobalEventListener
{

    // Start is called before the first frame update
    private BoltEntity _player;
    private ArrayList entitys = new ArrayList();
    void Start()
    {
        //BoltEntity player = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(45.0f, 17.8f, 109f), transform.rotation);
        //player.GetComponent<PlayerMovement>().setIsPlayer(true);       
        
    }

    
    [System.Obsolete]
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        BoltEntity entity = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(45.0f, 17.8f, 109f), transform.rotation);
        entity.GetComponent<PlayerMovement>().setIsPlayer(true);
        entitys.Add(entity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
