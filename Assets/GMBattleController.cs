using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class GMBattleController : NetworkBehaviour
{
    public GameObject _text_prefab_template;
    public Dropdown __playersSelection;
    public Button put_combat_button;
    public Button clear_combat;
    public Button new_turn;
    GameObject[] players;
    Dictionary<string,GameObject> inCombatDic = new Dictionary<string, GameObject>();
    int total_fighter=0;
    int current_fighter_turn=0;
    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer){
            retrievePlayers();
            put_combat_button.onClick.AddListener(addFighterInCombat);
            clear_combat.onClick.AddListener(clearCombat);
            new_turn.onClick.AddListener(advanceTurn);
            
        }
    }

    public void retrievePlayers(){
        players = GameObject.FindGameObjectsWithTag("Player");
        __playersSelection.ClearOptions();
        List<string> options = new List<string>();
        foreach (var item in players)
        {
            options.Add(item.GetComponent<CombatController>().pg_name);
            addFighterToDict(item.GetComponent<CombatController>().pg_name, item);
        }
        __playersSelection.AddOptions(options);
    }

    void addFighterInCombat(){
        string fighter = __playersSelection.options[__playersSelection.value].text;
        total_fighter +=1;
        instanciateFighterServerRpc(fighter);
    }

    void advanceTurn(){
        if(total_fighter > 1){
            if(current_fighter_turn >= total_fighter){
                current_fighter_turn = 0;
            }
            advanceTurnServerRpc(current_fighter_turn);
            current_fighter_turn +=1;
        }
        
    }

    void addFighterToDict(string fighter, GameObject fighter_go){
        Debug.Log("ADDING IN DICT : "+fighter);
        GameObject gg;
        bool iskeyin = inCombatDic.TryGetValue("fighter", out gg);
        if(!iskeyin)
            inCombatDic.Add(fighter, fighter_go);
    }
    
    void clearCombat(){
        inCombatDic.Clear();
        total_fighter = 0;
        current_fighter_turn=0;
        clearCombatServerRpc();
    }

    [ServerRpc]
    void instanciateFighterServerRpc(string fighter_name){
        instanciateFighterClientRpc(fighter_name);
    }

    [ClientRpc]
    void instanciateFighterClientRpc(string fighter_name){
        GameObject g;
        foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(pg.transform.parent.GetChild(2).gameObject.activeSelf){
                g = Instantiate(_text_prefab_template,pg.transform.parent.GetChild(2).GetChild(2).transform);
                g.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = fighter_name;
            }
        }

    }


    [ServerRpc]
    void clearCombatServerRpc(){
        clearCombatClientRpc();
    }

    [ClientRpc]
    void clearCombatClientRpc(){
        foreach (var pg in GameObject.FindGameObjectsWithTag("Player")){
            if(pg.transform.parent.GetChild(2).gameObject.activeSelf){
                GameObject mainCombatPanel = pg.transform.parent.GetChild(2).GetChild(2).gameObject;
                for(int i = 0; i < mainCombatPanel.transform.childCount;i++){
                    Destroy(mainCombatPanel.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    [ServerRpc]
    void advanceTurnServerRpc(int current_fighter_turn){
        advanceTurnClientRpc(current_fighter_turn);
    }

    [ClientRpc]
    void advanceTurnClientRpc(int current_fighter_turn){
        foreach (var pg in GameObject.FindGameObjectsWithTag("Player")){
            if(pg.transform.parent.GetChild(2).gameObject.activeSelf){
                GameObject mainCombatPanel = pg.transform.parent.GetChild(2).GetChild(2).gameObject;
                for(int i = 0; i < mainCombatPanel.transform.childCount;i++){
                    mainCombatPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
                mainCombatPanel.transform.GetChild(current_fighter_turn).GetChild(1).gameObject.SetActive(true);
            }
        }   
    }
}
