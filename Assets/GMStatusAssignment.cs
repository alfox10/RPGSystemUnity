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

    //private
    List<string> players_name;
    GameObject[] players_go;
    void Start()
    {
        players_name = new List<string>();

        retrievePlayers();
        setDDStatus();

        addStatusButton.onClick.AddListener(addStatusToPlayer);
    }

    void addStatusToPlayer(){
        Debug.Log("Adding Status to player");
        addStatusToPlayerServerRpc();
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
    void addStatusToPlayerServerRpc(){
        Debug.Log("Adding Status to player ::SERVER");
        addStatusToPlayerClientRpc();
    }

    [ClientRpc]
    void addStatusToPlayerClientRpc(){
        Debug.Log("Adding Status to player ::CLIENT");
        GameObject p_obj = players_go[__ddPlayers.value];
        GameObject status_panel = p_obj.transform.parent.GetChild(2).GetChild(1).gameObject;
        GameObject g;
        g = Instantiate(_statusTemplate, status_panel.transform);
        g.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("status_icons/burn", typeof(Sprite)) as Sprite;  
        g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "2/3";
    }
}
