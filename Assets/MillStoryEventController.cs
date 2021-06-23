using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStoryEventController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject basementLevel;
    public GameObject floorToMove;

    public void showBasement(){
        basementLevel.SetActive(true);
        floorToMove.transform.position = new Vector3(52.06993865966797f,0.0f,-44.017333984375f);
    }
}
