using UnityEngine;
using MLAPI;


public class PlayerNetwokHandler : NetworkBehaviour
{
    public GameObject[] _gameObjectsToDisable;

    void Start()
    {
        if(!IsLocalPlayer){
            foreach(GameObject go in _gameObjectsToDisable){
                go.transform.gameObject.SetActive(false);
            }
        }
    }


}
