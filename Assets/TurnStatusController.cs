using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class TurnStatusController : NetworkBehaviour
{
    public Text chat_text;

    public void advanceTurn(){
        if(transform.childCount > 1){
            for(int i = 1; i < transform.childCount; i++){
                string turn_left_text = transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text;
                int turn_left;
                int.TryParse(turn_left_text, out turn_left);
                if(turn_left > 1){
                    turn_left -= 1;
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = turn_left.ToString();
                } else  if(turn_left == 1){
                    
                    setChatResultServerRpc("\n"+transform.parent.parent.GetChild(5).gameObject.GetComponent<CombatController>().pg_name
                    +" - "+transform.GetChild(i).GetComponent<StatusNameController>().status_name+" ended");
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void setChatResultServerRpc(string text_to_add){

        setChatResultClientRpc(text_to_add);
    }

    [ClientRpc]
    public void setChatResultClientRpc(string text_to_add){
        chat_text = GameObject.FindGameObjectWithTag("chat").GetComponent<Text>();
        chat_text.text = chat_text.text + text_to_add;
    }

}
