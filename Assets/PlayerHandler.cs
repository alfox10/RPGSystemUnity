using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Prototyping;

public class PlayerHandler : NetworkBehaviour
{
    public float gravity = -9.8f;
    public float moveSpeed = 2f;
    private float last_x,last_z;
    private float waterSpeed = 1f;
    private float currentSpeed;
    private int _playersCount;

    private int token_id;
    private bool isGrounded=true;
    private string pg_name="";
    private int current_level_player_pos=1;



   

    
    void Start()
    {
        if(IsLocalPlayer){

            currentSpeed = moveSpeed;
            token_id = PlayerPrefs.GetInt("player_id");
            _playersCount = GameObject.FindGameObjectsWithTag("Player").Length;
            setMeshServerRpc(token_id);
            setPositionServerRpc(transform.position, transform.rotation);
        }
    }

    
    void Update (){
        if(IsLocalPlayer){
            if(!isGrounded){
                float posy = transform.position.y - (gravity*Time.deltaTime);
                transform.position = new Vector3(transform.position.x,posy,transform.position.z);
                Debug.Log("POSY : "+posy);
            }else {
            transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")* currentSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* currentSpeed);
            }
        }
    }

    void FixedUpdate() {
        if(IsLocalPlayer){
            if(pg_name == ""){
                if(transform.GetComponent<CombatController>().pg_name != ""){
                    pg_name = transform.GetComponent<CombatController>().pg_name;
                }
            }
            int _currentPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
            if(_playersCount > _currentPlayers){
                _playersCount = _currentPlayers;
               
            } else if(_playersCount < _currentPlayers){
                _playersCount = _currentPlayers;
                setMeshServerRpc(token_id);
                setPositionServerRpc(transform.position, transform.rotation);
            }
        }
    }

/*     private void TriggerHandler(Collider other){
        if(IsLocalPlayer){
            isGrounded = true;
            if(other.tag == "Player")
                return;
            bool isInCollisionArray = other.tag == "collider_obj";
            if(isInCollisionArray){
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")* currentSpeed * -1);
                transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* currentSpeed* -1);
                last_x = transform.position.x;
                last_z = transform.position.z;
            }else{
                if(other.tag == "water")
                    currentSpeed = waterSpeed;
                else if(last_x != transform.position.x || last_z != transform.position.z){
                        float y_pos = other.bounds.max.y+ (transform.lossyScale.y/2);
                        transform.position = new Vector3(transform.position.x, y_pos , transform.position.z);
                        currentSpeed = moveSpeed;
                    }
                
            }
        }
    } */
 
    private void OnTriggerEnter(Collider other){
        if(IsLocalPlayer){
            TriggerLevelVisibility(other);
        }
    }

    void TriggerLevelVisibility(Collider other){
        if(IsLocalPlayer && other.tag == "Trigger_levels"){
            GameObject level;
            if(other.GetComponent<FloorTriggerController>().level == null)
                return;
            level =  other.GetComponent<FloorTriggerController>().level;
            if(level.activeSelf){
                level.SetActive(false);
                current_level_player_pos -=1;
                manageEnemiesGuiOthersFloor(true, current_level_player_pos);
                   
            } else {
                level.SetActive(true);
                manageEnemiesGuiOthersFloor(false, current_level_player_pos);
                current_level_player_pos +=1;
               
            }
            comunicaSelfPositionToOthersServerRpc(current_level_player_pos,pg_name);
        }
    }


    void manageEnemiesGuiOthersFloor(bool beActive, int prev_lvl){
        GameObject p_l_g = GameObject.Find("level"+prev_lvl);
        for (int i = 0; i < p_l_g.transform.childCount; i++)
        {
            if(p_l_g.transform.GetChild(i).tag == "enemy_container"){
                for (int j = 1; j < p_l_g.transform.GetChild(i).transform.childCount; j++)
                {
                    p_l_g.transform.GetChild(i).transform.GetChild(j).gameObject.SetActive(beActive);
                }
            }
        }

    }

/*
    private void OnTriggerStay(Collider other){
        if(IsLocalPlayer){
            TriggerHandler(other);
        }
    }
    private void OnTriggerExit(Collider other) {
        transform.position = new Vector3(transform.position.x,transform.position.y * Time.deltaTime * gravity,transform.position.z);
        isGrounded = false;
    } */

//(RequireOwnership = false)
    [ServerRpc]
    void setMeshServerRpc(int t_id){
        setMeshClientRpc(t_id);
    }

    [ClientRpc]
    void setMeshClientRpc(int t_id){
        gameObject.GetComponent<Renderer>().material = Resources.Load("tokens/"+t_id, typeof(Material)) as Material; 
    }


    [ServerRpc(RequireOwnership = false)]
    void setPositionServerRpc(Vector3 pos, Quaternion rot){
        setPositionClientRpc(pos, rot);
    }

    [ClientRpc]
    void setPositionClientRpc(Vector3 pos, Quaternion rot){
        gameObject.GetComponent<NetworkTransform>().Teleport(pos, rot);
        
    }

    [ServerRpc]
    void comunicaSelfPositionToOthersServerRpc(int current_level_player_pos,string pg_name){
        comunicaSelfPositionToOthersClientRpc(current_level_player_pos,pg_name);
    }

    [ClientRpc]
    void comunicaSelfPositionToOthersClientRpc(int other_level_pos,string pg_name){
        foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(item.GetComponent<CombatController>().pg_name == pg_name){
                if(current_level_player_pos != other_level_pos){
                    item.GetComponent<Renderer>().enabled = false;
                    item.transform.GetChild(1).gameObject.SetActive(false);
                } else {
                    item.GetComponent<Renderer>().enabled = true;
                    item.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }


}
