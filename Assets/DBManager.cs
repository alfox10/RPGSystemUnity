using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class DBManager : MonoBehaviour
{

    public class Body{
        public string username;
        public string password;
    }

    public Button login;
    public InputField user;
    public InputField pass;
    
    // Start is called before the first frame update
    void Start()
    {
        //login.onClick.AddListener(submitData);
        login.onClick.AddListener(checkDataAndSend);
    }

    void checkDataAndSend(){
        if(!string.IsNullOrEmpty(user.text) && !string.IsNullOrEmpty(pass.text)){
             StartCoroutine(submitData());
        }

    }

    public class PGid{
        public int response;
    }

    IEnumerator submitData(){
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

        if (uwr.isNetworkError){
            Debug.Log("Error While Sending: " + uwr.error);
        } else {
            PGid pgId = new PGid();
            pgId = JsonUtility.FromJson<PGid>(uwr.downloadHandler.text);
            if(pgId.response != 0){
                PlayerPrefs.SetInt("player_id", pgId.response);
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
        }
       
    }
}
 