using System.Collections;
using System.Collections.Generic;
using  UnityEngine.UI;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class CombatController : NetworkBehaviour
{

    public GameObject combat_canvas;
    public GameObject _rowTemplate;
    private int _currentObjInCanvas;


    void Start(){
        _currentObjInCanvas = 0;
        combat_canvas = GameObject.FindGameObjectWithTag("combat_canvas");
        _rowTemplate = GameObject.FindGameObjectWithTag("_imageTemplate");
    }
    void Update()
    {
        if(IsHost){
            if(_currentObjInCanvas != UnitSelection.Instance.unitSelected.Count){
                _currentObjInCanvas = UnitSelection.Instance.unitSelected.Count;
                destroyUnitSelectionServerRpc();
                foreach(var incombat in UnitSelection.Instance.unitSelected){
                    string name = incombat.name;
                    if(incombat.transform.parent.GetChild(1).transform.gameObject.GetComponent<UIManager>() != null){
                        name = incombat.transform.parent.GetChild(1).transform.gameObject.GetComponent<UIManager>().getPlayerPrimaryInfo().name;
                    }
                    Debug.Log("PLAYER : "+incombat.name);
                    drawUnitSelectionServerRpc(name);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void drawUnitSelectionServerRpc(string name){
        drawUnitSelectionClientRpc(name);
    }

    [ClientRpc]
    void drawUnitSelectionClientRpc(string name){
        combat_canvas = GameObject.FindGameObjectWithTag("combat_canvas");
        _rowTemplate = GameObject.FindGameObjectWithTag("_imageTemplate");
        GameObject g;
        g = Instantiate(_rowTemplate, combat_canvas.transform);
        g.transform.GetChild(0).GetComponent<Text>().text = name;
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
