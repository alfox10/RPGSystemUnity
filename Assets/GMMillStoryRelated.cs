using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class GMMillStoryRelated : NetworkBehaviour
{
    public Button showBasementButton;


    void Start() {
        if(IsHost)
            showBasementButton.onClick.AddListener(showBasement);
    }

    void showBasement(){
        showBasementServerRpc();
    }

    [ServerRpc]
    void showBasementServerRpc(){
        showBasementClientRpc();
    }

    [ClientRpc]
    void showBasementClientRpc(){
       MillStoryEventController msec = GameObject.FindGameObjectWithTag("story_event_controller").GetComponent<MillStoryEventController>();
       msec.showBasement();
    }
}
