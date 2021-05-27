using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Button")]
    public GameObject _template;

    private GameObject[] spawn_points;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Looking for Spawn Points");
        spawn_points = GameObject.FindGameObjectsWithTag("spawn_point");
        GameObject g;
        int i = 0;
        foreach(GameObject spawn in spawn_points){
            g = Instantiate(_template, transform);
            g.transform.GetChild(0).GetComponent<Text>().text = spawn.name;
            Button b = g.GetComponent<Button>();
            b.onClick.AddListener(delegate{addTeleportCoordinates(spawn);});
            i +=1;
        }
        Destroy(_template);

    }
    private void addTeleportCoordinates(GameObject spawn){
        player.position = spawn.transform.position;
    }

    // Update is called once per frame
}
