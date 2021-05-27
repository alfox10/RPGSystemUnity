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
        foreach(GameObject sl in spawn_levels){
            sl.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
