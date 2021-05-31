using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerNetwokHandler : NetworkBehaviour
{
    public GameObject[] _gameObjectsToDisable;
    public BoxCollider b_coll;

    void Start()
    {
        Debug.Log("Clearing obj in common");
        if(!IsLocalPlayer){
            foreach(GameObject go in _gameObjectsToDisable){
                go.transform.gameObject.SetActive(false);
            }
           // b_coll.enabled = false;
        }
    }


}
