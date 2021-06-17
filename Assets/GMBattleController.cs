using System.Collections;
using System.Linq;
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
    public InputField rolled_value;
    GameObject[] players;
    int total_fighter = 0;
    int current_fighter_turn = 0;
    int retrieved_fighters = 0;
    IOrderedEnumerable<KeyValuePair<string,int>> sortedDict;
    Dictionary<string,int> turn_to_name = new Dictionary<string,int>();
    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            //retrievePlayers();
            put_combat_button.onClick.AddListener(addFighterInCombat);
            clear_combat.onClick.AddListener(clearCombat);
            new_turn.onClick.AddListener(advanceTurn);
            retrieveSelectedCombatFighter();

        }
    }

    void Update() {
        retrieveSelectedCombatFighter();
    }

    void retrieveSelectedCombatFighter(){
        if(retrieved_fighters < UnitSelection.Instance.unitSelected.Count){
            __playersSelection.ClearOptions();
            List<string> options = new List<string>();
            foreach (var sel in  UnitSelection.Instance.unitSelected)
            {
                if(sel.tag == "Player")
                    options.Add(sel.GetComponent<CombatController>().pg_name);
                else
                    options.Add(sel.name);
            }
            retrieved_fighters = UnitSelection.Instance.unitSelected.Count;
            __playersSelection.AddOptions(options);
        }
    }

/*     public void retrievePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        __playersSelection.ClearOptions();
        List<string> options = new List<string>();
        foreach (var item in players)
        {
            options.Add(item.GetComponent<CombatController>().pg_name);
        }
        __playersSelection.AddOptions(options);
    } */

    void addFighterInCombat()
    {
        string fighter = __playersSelection.options[__playersSelection.value].text;
        if(rolled_value.text == ""){
            Debug.Log("You must set rolled value field");
            return;
        }
        int rolled_value_int;
        int.TryParse(rolled_value.text, out rolled_value_int);
        turn_to_name.Add(fighter,rolled_value_int);
        total_fighter += 1;
        sortedDict = from entry in turn_to_name orderby entry.Value descending select entry;
        clearCombatServerRpc();
        foreach (var item in sortedDict)
        {
            Debug.Log("SORTED DICT : "+item.Value+ " "+item.Key);
            instanciateFighterServerRpc(item.Value+" - "+item.Key);
        }
        
    }

    void advanceTurn()
    {
        if (total_fighter > 1 && sortedDict != null)
        {
            if (current_fighter_turn >= total_fighter)
            {
                current_fighter_turn = 0;
            }
            //foreach (var item in UnitSelection.Instance.unitSelected)
            foreach (var item in sortedDict)
            {
                stopAllAnimServerRpc(item.Key);
            }
            advanceTurnServerRpc(current_fighter_turn, sortedDict.ElementAt(current_fighter_turn).Key);
            current_fighter_turn += 1;
        }

    }

    void clearCombat()
    {
        foreach (var item in UnitSelection.Instance.unitSelected)
        {
            string curr_name_to_stop="";
            if(item.tag=="Player"){
                curr_name_to_stop = item.GetComponent<CombatController>().pg_name;
            } else {
                curr_name_to_stop = item.name;
            }
            stopAllAnimServerRpc(curr_name_to_stop);
        }
        UnitSelection.Instance.DeselectAll();
        total_fighter = 0;
        retrieved_fighters = 0;
        current_fighter_turn = 0;
        __playersSelection.ClearOptions();
        turn_to_name.Clear();
        sortedDict = null;
        
        clearCombatServerRpc();
    }

    [ServerRpc]
    void instanciateFighterServerRpc(string fighter_name)
    {
        instanciateFighterClientRpc(fighter_name);
    }

    [ClientRpc]
    void instanciateFighterClientRpc(string fighter_name)
    {
        GameObject g;
        foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (pg.transform.parent.GetChild(2).gameObject.activeSelf)
            {
                g = Instantiate(_text_prefab_template, pg.transform.parent.GetChild(2).GetChild(2).transform);
                g.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = fighter_name;
            }
        }

    }


    [ServerRpc]
    void clearCombatServerRpc()
    {
        clearCombatClientRpc();
    }

    [ClientRpc]
    void clearCombatClientRpc()
    {
        foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (pg.transform.parent.GetChild(2).gameObject.activeSelf)
            {
                GameObject mainCombatPanel = pg.transform.parent.GetChild(2).GetChild(2).gameObject;
                for (int i = 0; i < mainCombatPanel.transform.childCount; i++)
                {
                    Destroy(mainCombatPanel.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] 
    void advanceTurnServerRpc(int current_fighter_turn, string turn_name)
    {
        advanceTurnClientRpc(current_fighter_turn, turn_name);
    }

    [ClientRpc]
    void advanceTurnClientRpc(int current_fighter_turn, string turn_name)
    {
        GameObject fgt = null;

        Debug.Log("Starting new Turn for "+turn_name);
        if(GameObject.Find(turn_name) == null){
            //Player
            foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(pg.GetComponent<CombatController>().pg_name == turn_name)
                    fgt = pg;
            }

        } else {
            //enemy
            fgt = GameObject.Find(turn_name);
        }

        if(fgt != null){
          Debug.Log("Found valid fgt : "+ fgt.name);
          fgt.transform.GetChild(0).gameObject.GetComponent<SelectionMeshMovementController>().isAnim = true;
          foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
          {
            if (pg.transform.parent.GetChild(2).gameObject.activeSelf)
                {
                    Debug.Log("advance GUI Combat");
                    GameObject mainCombatPanel = pg.transform.parent.GetChild(2).GetChild(2).gameObject;
                    for (int i = 0; i < mainCombatPanel.transform.childCount; i++)
                    {
                        mainCombatPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    }
                    mainCombatPanel.transform.GetChild(current_fighter_turn).GetChild(1).gameObject.SetActive(true);
                }
          }

        }else{
            Debug.Log("nothing found ::: advanceTurnClientRpc");
        }
    }

    [ServerRpc]
    void stopAllAnimServerRpc(string name){
        stopAllAnimClientRpc(name);
    }

    [ClientRpc]
    void stopAllAnimClientRpc(string name){
        GameObject fgt = null;

        Debug.Log("Cleaning anim for "+name);
        if(GameObject.Find(name) == null){
            //Player
            foreach (var pg in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(pg.GetComponent<CombatController>().pg_name == name)
                    fgt = pg;
            }

        } else {
            //enemy
            fgt = GameObject.Find(name);
        }

        if(fgt != null){
            fgt.transform.GetChild(0).gameObject.GetComponent<SelectionMeshMovementController>().isAnim = false;
        }else{
            Debug.Log("nothing found ::: stopAllAnimClientRpc");
        }
    }
}
