using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerHandler : NetworkBehaviour
{

    public float moveSpeed = 2f;
    private float last_x,last_z;
    private float waterSpeed = 1f;
    private float currentSpeed;
    private CharacterController cc;
    private int _playersCount;


   

    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer){
            currentSpeed = moveSpeed;
            cc = GetComponent<CharacterController>();
            int token_id = PlayerPrefs.GetInt("player_id");
            _playersCount = GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log("Message from : "+token_id+" current players : "+_playersCount);
            setMeshServerRpc(token_id);
        }
    }

    // Update is called once per frame
    void Update (){
        if(IsLocalPlayer){
            transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")* currentSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* currentSpeed);
        }
    }

    void FixedUpdate() {
        if(IsLocalPlayer){
            int _currentPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
            if(_playersCount < _currentPlayers){
                _playersCount = _currentPlayers;
                int token_id = PlayerPrefs.GetInt("player_id");
                Debug.Log("Message from : "+token_id+" current players : "+_playersCount);
                setMeshServerRpc(token_id);
            }
        }
    }

    private void TriggerHandler(Collider other){
        if(IsLocalPlayer){
            bool isInCollisionArray = other.tag == "collider_obj";
            if(isInCollisionArray){
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical")* currentSpeed * -1);
                transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* currentSpeed* -1);
                last_x = transform.position.x;
                last_z = transform.position.z;
            }else{
                if(other.tag == "water")
                    currentSpeed = waterSpeed;
                else
                    currentSpeed = moveSpeed;
                
                if(other.tag != "Player"){
                    if(last_x != transform.position.x || last_z != transform.position.z){
                        float y_pos = other.bounds.max.y+ (transform.lossyScale.y/2);
                        transform.position = new Vector3(transform.position.x, y_pos , transform.position.z);
                    }
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

}
