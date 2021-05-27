using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceButtonController : MonoBehaviour
{

    [Header("Dice Info")]
    public GameObject npcRoll;
    public string diceName;
    private NPCRollController npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = npcRoll.GetComponent<NPCRollController>();
        Button bt = GetComponent<Button>();
        bt.onClick.AddListener(delegate{setDice(diceName);});
    }

    void setDice(string dn){
        npc.SetSprite(dn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
