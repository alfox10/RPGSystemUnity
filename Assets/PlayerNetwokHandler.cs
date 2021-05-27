using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerNetwokHandler : NetworkBehaviour
{
    public GameObject[] _gameObjectsToDisable;
    public BoxCollider b_coll;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsLocalPlayer){
            foreach(GameObject go in _gameObjectsToDisable){
                go.transform.gameObject.SetActive(false);
            }
            b_coll.enabled = false;
        }
    }

}
