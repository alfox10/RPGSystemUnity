using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class FloorTriggerController : NetworkBehaviour
{
    public GameObject level;
/*     private void OnTriggerEnter(Collider other){
        if(IsLocalPlayer){
            if(other.tag == "Player")
                manageLevelVisibility();
        }
    }

    void manageLevelVisibility(){
        if(level.activeSelf){
            level.SetActive(false);
        } else {
            level.SetActive(true);
        }
    } */

}
