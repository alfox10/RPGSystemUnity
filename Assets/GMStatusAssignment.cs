using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class GMStatusAssignment : NetworkBehaviour
{

    [Header("BUFF List")]
    public string[] buffs;

    [Header("DEBUFF List")]
    public string[] debuffs;

    [Header("References")]
    public Dropdown __ddPlayers;
    public Dropdown __ddStatus;
    public Button addStatusButton;
    public GameObject _statusTemplate;
    public InputField turns;

    //private
    List<string> players_name;
    GameObject[] players_go;
    GameObject p_target;
    void Start()
    {
        players_name = new List<string>();

        retrievePlayers();
        setDDStatus();

        addStatusButton.onClick.AddListener(addStatusToPlayer);
    }

    void addStatusToPlayer(){
        Debug.Log("Adding Status to player");
        p_target = players_go[__ddPlayers.value];
        if(turns.text == ""){
            Debug.Log("No Turns Set");
        }else{
            addStatusToPlayerServerRpc(p_target.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name,__ddStatus.options[__ddStatus.value].text, turns.text);
        }
    }

    void setDDStatus(){
        List<string> __ddStatusOptions = new List<string>();
        foreach (var debuff in debuffs)
        {
            __ddStatusOptions.Add(debuff);
        }
        
        foreach (var buff in buffs)
        {
            __ddStatusOptions.Add(buff);
        }

        __ddStatus.ClearOptions();
        __ddStatus.AddOptions(__ddStatusOptions);

    }

    public void retrievePlayers()
    {
        players_name.Clear();
        players_go = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players_go)
        {
            players_name.Add(player.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name);
        }
        __ddPlayers.ClearOptions();
        __ddPlayers.AddOptions(players_name);
    }

    [ServerRpc]
    void addStatusToPlayerServerRpc(string p_name, string icon, string turncount){
        Debug.Log("Adding Status to player ::SERVER");
        addStatusToPlayerClientRpc(p_name, icon,turncount);
    }

    [ClientRpc]
    void addStatusToPlayerClientRpc(string p_name, string icon, string turncount){
        GameObject[] ps = GameObject.FindGameObjectsWithTag("Player");
        foreach (var pg in ps)
        {
            if(p_name == pg.GetComponent<CombatController>().pg_name){
                GameObject status_panel = pg.transform.parent.GetChild(2).GetChild(1).gameObject;
                GameObject g;
                g = Instantiate(_statusTemplate, status_panel.transform);
                g.GetComponent<StatusNameController>().status_name = icon;
                g.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("status_icons/"+icon, typeof(Sprite)) as Sprite;  
                g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = turncount;
            }
        }
    
    }
}
