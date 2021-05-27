using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCRollController : MonoBehaviour
{
    [Header("Dice Info")]
    public GameObject diceGameCtl;
    public float roll_time;
    public Text npc_name;
    public Text chat_text;

    //private
    private DiceGameController dgc;
    private Image im;
    private bool isRolling;
    private float local_roll_time;
    private int idx;
    private string current_dice_type;
    private int diceResult;

    void Start()
    {
        current_dice_type = "d20";
        dgc = diceGameCtl.GetComponent<DiceGameController>();
        im = GetComponent<Image>();
        im.sprite = dgc.GetSprite(0);
        isRolling = false;
        local_roll_time = roll_time;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRolling)
            rollDice();
    }

    void rollDice(){
       // if((int)(Time.deltaTime*100) % 2 == 0){
            int min_result = dgc.GetDiceInfo(current_dice_type).startImage;
            int max_result = dgc.GetDiceInfo(current_dice_type).endImage;
            idx = Random.Range(min_result,max_result);
            im.sprite = dgc.GetSprite(idx);
       // }
        local_roll_time -= Time.deltaTime*3;

        if(local_roll_time < 0){
            diceResult = (idx+1) - dgc.GetDiceInfo(current_dice_type).startImage;
            isRolling = false;
            chat_text.text= chat_text.text+"\n"+npc_name.text+" rolled "+diceResult+" on a "+current_dice_type;
            local_roll_time = roll_time;
        }
    }

    public void SetSprite(string diceType){
        current_dice_type = diceType;
        im.sprite = dgc.GetSprite(dgc.GetDiceInfo(current_dice_type).startImage);
        isRolling = true;
    }
}
