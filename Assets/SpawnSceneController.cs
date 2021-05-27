using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSceneController : MonoBehaviour
{

    private GameObject[] spawn_levels;

    void Start()
    {
        Debug.Log("Actvating levels");
        spawn_levels = GameObject.FindGameObjectsWithTag("LevelToSpawn");
        Debug.Log("Actvating levels : "+spawn_levels.Length);
        foreach(GameObject sl in spawn_levels){
            Debug.Log(sl.name);
            Debug.Log(sl.transform.GetChild(0).name);
            sl.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
