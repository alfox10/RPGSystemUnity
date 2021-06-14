using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class InventoryManager : MonoBehaviour
{

    public GameObject[] inv_icons;

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
        foreach (ItemFormat item in ifl.itemFormatList)
        {
            int idx = item.is_equipped;
            switch (idx)
            {
                case 0:
                    inv_icons[idx].GetComponent<Text>().text = item.qt.ToString();
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    inv_icons[idx].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("item_icons/"+item.icon, typeof(Sprite)) as Sprite;
                    inv_icons[idx].transform.GetChild(0).GetComponent<TooltipTrigger>().content = item.effect;
                    inv_icons[idx].transform.GetChild(0).GetComponent<TooltipTrigger>().isUsed = true;
                    inv_icons[idx].transform.GetChild(0).GetComponent<TooltipTrigger>().header = item.description;
                    inv_icons[idx].transform.GetChild(0).GetComponent<TooltipTrigger>().image_content = item.icon;
                    break;
                default:
                    inv_icons[idx].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("item_icons/"+item.icon, typeof(Sprite)) as Sprite;
                    inv_icons[idx].GetComponent<TooltipTrigger>().content = item.effect;
                    inv_icons[idx].GetComponent<TooltipTrigger>().header = item.description;
                    inv_icons[idx].GetComponent<TooltipTrigger>().isUsed = true;
                    inv_icons[idx].GetComponent<TooltipTrigger>().image_content = item.icon;
                    inv_icons[idx].transform.GetChild(1).gameObject.SetActive(true);
                    inv_icons[idx].transform.GetChild(1).GetComponent<Text>().text = item.qt.ToString();
                    break;
            }
        }
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
        public int is_equipped;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
