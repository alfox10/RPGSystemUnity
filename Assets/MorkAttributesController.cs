using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MLAPI;
using UnityEngine.UI;

public class MorkAttributesController : NetworkBehaviour
{
 
    public Text agility;
    public Text presence;
    public Text strength;
    public Text toughness;

    PGAttrs pGAttrs;
    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer){
            StartCoroutine(setMorkAttr(PlayerPrefs.GetInt("player_id")));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class PGid{
        public int id;
    }

    public class PGAttrs{
        public int agility;
        public int presence;
        public int strength;
        public int toughness;
    }

    IEnumerator setMorkAttr(int p_id){
        PGid jsonbody = new PGid();
        jsonbody.id = p_id;
        string json = JsonUtility.ToJson(jsonbody);
        UnityWebRequest uwr = new UnityWebRequest("https://rpgsystem.alfox10.repl.co/api/v1/mork_attributes", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError){
            Debug.Log("Error While Sending: " + uwr.error);
        } else {
            Debug.Log(uwr.downloadHandler.text);
            pGAttrs = new PGAttrs();
            pGAttrs = JsonUtility.FromJson<PGAttrs>(uwr.downloadHandler.text);
            changeGuiAttrInfo(pGAttrs);
        }
        if (uwr != null)
        {
            uwr.Dispose();
            uwr = null;
        }
    }

    void changeGuiAttrInfo(PGAttrs p){
        agility.text = p.agility.ToString();
        presence.text = p.presence.ToString();
        strength.text = p.strength.ToString();
        toughness.text = p.toughness.ToString();
    }
}
