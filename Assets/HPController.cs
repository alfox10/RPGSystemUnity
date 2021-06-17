using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public int MAX_HP;
    public Image im_sprite;
    //public Sprite dead_sprite;
    public NPCRollController nPCRoll;
    private Text tx;
    private int current_hp;
    private bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        current_hp = MAX_HP;
        tx = GetComponent<Text>();
        SetText();
        isAlive = true;
    }

    private void SetText(){
        tx.text = current_hp+"/"+MAX_HP;   
    }

    public void AddHp(){
        if(current_hp < MAX_HP && isAlive){
            current_hp +=1;
            SetText();
        }
    }

    public void RemoveHp(){
        if(current_hp > 0 && isAlive){
            current_hp -=1;
            SetText();
        }

        if(current_hp == 0 && isAlive){
            Debug.Log("NPC Dead");
            isAlive = false;
            nPCRoll.setNPCDead();
        }
    }

}
