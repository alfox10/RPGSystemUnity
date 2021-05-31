using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;

public class LoginController : MonoBehaviour
{
    [Header("Object Arrays")]
    public GameObject[] _objToActivateAll;

    public GameObject[] _objToActivateHostOnly;
    public GameObject[] _objToDeactivate;

    [Header("Gui controller")]
    public InputField user;
    public InputField pass;
    public Dropdown game;
    public Text error_handler;


    void Start(){
        error_handler.text = "";
    }
    

    private void connectAndLogin(string whoami){
        if(whoami == "host"){ 
                NetworkManager.Singleton.StartHost();
                foreach ( GameObject go in _objToActivateHostOnly){
                        go.SetActive(true); 
                }
                foreach ( GameObject go in _objToActivateAll){
                        go.SetActive(true); 
                }
                foreach ( GameObject go in _objToDeactivate){
                    go.SetActive(false);
                }


        } else {
                NetworkManager.Singleton.StartClient();
                foreach ( GameObject go in _objToActivateAll){
                        go.SetActive(true); 
                }
                foreach ( GameObject go in _objToDeactivate){
                    go.SetActive(false);
                }
        }

    }


     public void checkDataAndSendHost(){
        if(!string.IsNullOrEmpty(user.text) && !string.IsNullOrEmpty(pass.text)){
             StartCoroutine(submitData("host"));
        }else{
            error_handler.text = "please fill the requested fields";
        }

    }

    
     public void checkDataAndSendClient(){
        if(!string.IsNullOrEmpty(user.text) && !string.IsNullOrEmpty(pass.text)){
             StartCoroutine(submitData("client"));
        }else{
            error_handler.text = "please fill the requested fields";
        }

    }

    public class PGid{
        public int response;
    }

    public class Body{
        public string username;
        public string password;
    }

    IEnumerator submitData(string whoami){
        Body jsonbody = new Body();
        jsonbody.username = user.text;
        jsonbody.password = pass.text;
        string json = JsonUtility.ToJson(jsonbody);
        var uwr = new UnityWebRequest("https://rpgsystem.alfox10.repl.co/api/v1/credentials", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError){
            Debug.Log("Error While Sending: " + uwr.error);
        } else {
            PGid pgId = new PGid();
            pgId = JsonUtility.FromJson<PGid>(uwr.downloadHandler.text);
            if(pgId.response != 0){
                PlayerPrefs.SetInt("player_id", pgId.response);
                PlayerPrefs.SetString("game", game.options[game.value].text);
                connectAndLogin(whoami);
            }else{
                Debug.Log("not valid user and/or pass");
                error_handler.text = "not valid user and/or pass";
            }
        }
       
    }
}
