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
    private int current_pg_in_game=0;

    void Update()
    {
        if(IsLocalPlayer){
            GameObject[] pgs = GameObject.FindGameObjectsWithTag("Player");
            if(current_pg_in_game < pgs.Length){
                Debug.Log("init PG NAME ::");
                foreach (var item in pgs)
                {
                    if(item.transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats != null){
                        Debug.Log("not more null ::: "+item.transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats.name);
                        setNameForPlayerServerRpc(item.transform.parent.GetChild(1).gameObject.GetComponent<UIManager>().pgStats.name);
                        current_pg_in_game = pgs.Length;
                    }
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
        Debug.Log("NAME :: "+gameObject.transform.parent.GetChild(5).gameObject.name);
        gameObject.transform.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name = pname;
    }
}
