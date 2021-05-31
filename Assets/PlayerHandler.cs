using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Prototyping;

public class PlayerHandler : NetworkBehaviour
{

    public float moveSpeed = 2f;
    private float last_x,last_z;
    private float waterSpeed = 1f;
    private float currentSpeed;
    private int _playersCount;

    private int token_id;


   

    
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
            transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")* currentSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* currentSpeed);
        }
    }

    void FixedUpdate() {
        if(IsLocalPlayer){
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

    private void TriggerHandler(Collider other){
        if(IsLocalPlayer){
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
    }

    private void OnTriggerEnter(Collider other){
        if(IsLocalPlayer){
            TriggerHandler(other);
        }
    }

    private void OnTriggerStay(Collider other){
        if(IsLocalPlayer){
            TriggerHandler(other);
        }
    }

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

}
