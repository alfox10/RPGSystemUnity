using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class InventoryManager : MonoBehaviour
{

    private int player_id;

    [Serializable]
    public struct Item
    {
        public Sprite item_icon;
        public string item_name;
        public string item_effect;
        public string item_qt;
    }


    // Start is called before the first frame update
    void Start()
    {
        player_id = PlayerPrefs.GetInt("player_id");
        //im.sprite = Resources.Load("item_icons/rings", typeof(Sprite)) as Sprite;
        retrievePlayerInventory(player_id);
        Debug.Log("Init Inventory");
        
    }
    
    void formatInventory(ListItemFormat ifl){

        GameObject item_template = transform.GetChild(0).gameObject;
        GameObject g;
        foreach (ItemFormat item in ifl.itemFormatList)
        {
            g = Instantiate(item_template, transform);
            g.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load("item_icons/"+item.icon, typeof(Sprite)) as Sprite;
            g.transform.GetComponent<TooltipTrigger>().content = item.effect;
            g.transform.GetChild(1).GetComponent<Text>().text = item.description;
            g.transform.GetChild(2).GetComponent<Text>().text = item.effect.Split(char.Parse(" "))[0]+" "+item.effect.Split(char.Parse(" "))[1]+"...";
            g.transform.GetChild(3).GetComponent<Text>().text = "Qt: "+item.qt;
        }

        Destroy(item_template);
    }

    void retrievePlayerInventory(int p_id){
        StartCoroutine(retrieveInventoryFromAPI(p_id));
    }

    public class PGid{
        public int id;
    }

    IEnumerator retrieveInventoryFromAPI(int p_id){
        PGid jsonbody = new PGid();
        jsonbody.id = p_id;
        string json = JsonUtility.ToJson(jsonbody);
        var uwr = new UnityWebRequest("https://rpgsystem.alfox10.repl.co/api/v1/inventory", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError){
            Debug.Log("Error While Sending: " + uwr.error);
        } else {
            
            ListItemFormat ifl = new ListItemFormat();
            ifl = JsonUtility.FromJson<ListItemFormat>(uwr.downloadHandler.text);
            formatInventory(ifl);
        }
    }
    [Serializable]
    public class ListItemFormat{
        public ItemFormat[] itemFormatList;
    }

    [Serializable]
    public class ItemFormat
    {
        public string description;
        public string effect;
        public int qt;
        public string icon;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
