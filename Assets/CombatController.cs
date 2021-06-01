using System.Collections;
using System.Collections.Generic;
using  UnityEngine.UI;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.Networking;
using MLAPI.NetworkVariable;
public class CombatController : NetworkBehaviour
{

    public GameObject combat_canvas;
    public GameObject _rowTemplate;

    public string pg_name;
    private int _currentObjInCanvas;




    void Start(){
        if(IsLocalPlayer){
        _currentObjInCanvas = 0;
        combat_canvas = GameObject.FindGameObjectWithTag("combat_canvas");
        _rowTemplate = GameObject.FindGameObjectWithTag("_imageTemplate");
        }
    }

    void Update()
    {
        if(IsLocalPlayer){
            if(pg_name == "" && transform.gameObject.tag == "Player"){
                if(transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats != null)
                    setNameForPlayerServerRpc(transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats.name);
            }
        }
        if(IsHost){
            if(_currentObjInCanvas != UnitSelection.Instance.unitSelected.Count){
                _currentObjInCanvas = UnitSelection.Instance.unitSelected.Count;
                destroyUnitSelectionServerRpc();
                foreach(var incombat in UnitSelection.Instance.unitSelected){
                    string cur_p_name = incombat.name;
                    Debug.Log("TAG : "+incombat.tag);
                    if(incombat.tag == "Player"){
                        cur_p_name = incombat.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name;
                    }
                    drawUnitSelectionServerRpc(cur_p_name);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void setNameForPlayerServerRpc(string pname){
        setNameForPlayerClientRpc(pname);
    }

    [ClientRpc]
    void setNameForPlayerClientRpc(string pname){
        Debug.Log("Comunicating name ::PNAME-CLIENT");
        gameObject.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name = pname;
    }


    [ServerRpc(RequireOwnership = false)]
    void drawUnitSelectionServerRpc(string incombat){
        drawUnitSelectionClientRpc(incombat);
    }

    [ClientRpc]
    void drawUnitSelectionClientRpc(string incombat){
        combat_canvas = GameObject.FindGameObjectWithTag("combat_canvas");
        _rowTemplate = GameObject.FindGameObjectWithTag("_imageTemplate");
        GameObject g;
        g = Instantiate(_rowTemplate, combat_canvas.transform);
        g.transform.GetChild(0).GetComponent<Text>().text = incombat;
        g.GetComponent<Image>().color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
    }

    [ServerRpc(RequireOwnership = false)]
    void destroyUnitSelectionServerRpc(){
        destroyUnitSelectionClientRpc();
    }

    [ClientRpc]
    void destroyUnitSelectionClientRpc(){
        combat_canvas = GameObject.FindGameObjectWithTag("combat_canvas");
        for(int i = 0; i < combat_canvas.transform.childCount; i++){
            Destroy(combat_canvas.transform.GetChild(i).gameObject);
        }
    
    }
}
