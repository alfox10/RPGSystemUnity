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

    [Header("Teleport Variables")]
    public float minX = -1.5f;
    public float maxX = 1.5f;
    public float minY = -1.5f;
    public float maxY = 1.5f;

    private GameObject[] spawn_points;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Looking for Spawn Points");
        spawn_points = GameObject.FindGameObjectsWithTag("spawn_point");
        GameObject g;
        foreach(GameObject spawn in spawn_points){
            g = Instantiate(_template, transform);
            g.transform.GetChild(0).GetComponent<Text>().text = spawn.name;
            Button b = g.GetComponent<Button>();
            b.onClick.AddListener(delegate{addTeleportCoordinates(spawn);});
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
            Vector3 offset = new Vector3(Random.Range(minX,maxX),0,Random.Range(minY,maxY));
            pg.transform.position = spawn+offset;
        }
    }

}
