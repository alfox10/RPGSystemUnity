using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class NPCOnlyGMGUI : NetworkBehaviour
{

    public GameObject[] _onlyGM;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsHost && IsLocalPlayer){
        foreach(GameObject go in _onlyGM){
            go.transform.gameObject.SetActive(false);
        }
        }
        
    }

}
