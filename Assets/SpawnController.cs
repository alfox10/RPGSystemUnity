using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    public Transform[] spawns;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawning Player in city");
        player.position = spawns[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
