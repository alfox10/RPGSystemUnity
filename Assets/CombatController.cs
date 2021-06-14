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

    public string pg_name;

    void Update()
    {
        if(IsLocalPlayer){
            if(pg_name == "" && transform.gameObject.tag == "Player"){
                if(transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats != null)
                    setNameForPlayerServerRpc(transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats.name);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void setNameForPlayerServerRpc(string pname){
        setNameForPlayerClientRpc(pname);
    }

    [ClientRpc]
    void setNameForPlayerClientRpc(string pname){
        gameObject.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name = pname;
    }
}
