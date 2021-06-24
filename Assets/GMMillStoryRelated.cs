using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class GMMillStoryRelated : NetworkBehaviour
{
    public Button showBasementButton;
    public Button updateSpeedButton;
    public InputField newSpeedValueField;


    void Start() {
        if(IsHost){
            showBasementButton.onClick.AddListener(showBasement);
            updateSpeedButton.onClick.AddListener(changePGSpeed);
        }
    }

    void changePGSpeed(){
        if(newSpeedValueField.text == ""){
            return;
        }

        int speed;
        int.TryParse(newSpeedValueField.text, out speed);
        changeSpeedServerRpc(speed);

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

    [ServerRpc]
    void changeSpeedServerRpc(int speed){
        changeSpeedClientRpc(speed);
    }

    [ClientRpc]
    void changeSpeedClientRpc(int speed){
        Debug.Log(gameObject.name);
        Debug.Log(gameObject.transform.parent.name);
         //.parent.GetChild(5).GetComponent<PlayerHandler>().moveSpeed = speed;
    }
}
