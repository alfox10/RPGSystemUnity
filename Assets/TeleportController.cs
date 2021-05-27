using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class TeleportController : NetworkBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Button")]
    public GameObject _template;

    private GameObject[] spawn_points;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Looking for Spawn Points");
        spawn_points = GameObject.FindGameObjectsWithTag("spawn_point");
        GameObject g;
        int i = 0;
        foreach(GameObject spawn in spawn_points){
            g = Instantiate(_template, transform);
            g.transform.GetChild(0).GetComponent<Text>().text = spawn.name;
            Button b = g.GetComponent<Button>();
            b.onClick.AddListener(delegate{addTeleportCoordinates(spawn);});
            i +=1;
        }
        Destroy(_template);

    }
    private void addTeleportCoordinates(GameObject spawn){ 
        Vector3 newpos = spawn.transform.position;
        TeleportAllPlayersClientRpc(newpos);
    }


    [ClientRpc]
    void TeleportAllPlayersClientRpc(Vector3 spawn){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject pg in players){
            Vector3 offset = new Vector3(Random.Range(-3,3),0,Random.Range(-3,3));
            pg.transform.position = spawn+offset;
        }
    }

    // Update is called once per frame
}
