using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGameController : MonoBehaviour
{

    public Sprite[] face_array;


    public class DiceInfo{
        public int startImage;
        public int endImage;
    }

    public Sprite GetSprite(int idx){
        return face_array[idx];
    }

    public DiceInfo GetDiceInfo(string diceType){
        DiceInfo dice = new DiceInfo();
        switch (diceType){
            case "d20":
                dice.startImage = 0;
                dice.endImage = 20;
                break;
            case "d4":
                dice.startImage = 20;
                dice.endImage = 24;
              break;
            case "d6":
                dice.startImage = 24;
                dice.endImage = 30;
              break;
            case "d8":
                dice.startImage = 30;
                dice.endImage = 38;
              break;
            case "d10":
                dice.startImage = 38;
                dice.endImage = 48;
              break;
            case "d12":
                dice.startImage = 48;
                dice.endImage = 60;
              break;
          default:
                dice.startImage = 0;
                dice.endImage = 20;
              break;
      }

      return dice;
    }

}
