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
    private int _playersCount;


   

    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer){
            currentSpeed = moveSpeed;
            int token_id = PlayerPrefs.GetInt("player_id");
            _playersCount = GameObject.FindGameObjectsWithTag("Player").Length;
            setMeshServerRpc(token_id);
            setPositionServerRpc(transform.localPosition, token_id);
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
            if(_playersCount > _currentPlayers){
                _playersCount = _currentPlayers;
               
            } else if(_playersCount < _currentPlayers){
                _playersCount = _currentPlayers;
                int token_id = PlayerPrefs.GetInt("player_id");
                setMeshServerRpc(token_id);
                setPositionServerRpc(transform.localPosition, token_id);
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


    [ServerRpc]
    void setPositionServerRpc(Vector3 pos, int tok){
        setPositionClientRpc(pos, tok);
    }

    [ClientRpc]
    void setPositionClientRpc(Vector3 pos, int tok){
        Debug.Log("Moving "+tok+"\n NewPos : "+pos+"  \n LocalPos : "+gameObject.transform.localPosition);
        gameObject.transform.localPosition = pos;
        
    }

}
