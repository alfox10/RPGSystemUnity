using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSceneController : MonoBehaviour
{


    public GameObject[] triggers_levels_to_spawn;
    private GameObject[] spawn_levels;

    void Start()
    {

        Debug.Log("Actvating levels");
        spawn_levels = GameObject.FindGameObjectsWithTag("LevelToSpawn");
        foreach(GameObject sl in spawn_levels){
            for(int i=0; i< sl.transform.childCount;i++){
                if(sl.transform.GetChild(i).tag == "LevelToSpawn")
                    sl.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
