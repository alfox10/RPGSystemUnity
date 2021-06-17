using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class UnitSelection : NetworkBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    private static UnitSelection _instance;
    public static UnitSelection Instance { get { return _instance; } }


    void Awake()
    {
        if(IsLocalPlayer)
            Debug.Log("LOCAL P :::::");
        if(IsHost)
            Debug.Log("HOST ::::");

        if(IsClient)
            Debug.Log("CLIENT ::::");

        if (_instance == null)
        {
            _instance = this;
        }

    }


    public void ClickSelect(GameObject unitToAdd)
    {

        //DeselectAll();
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            showSelectionCircleServerRpc(getRealName(unitToAdd), unitToAdd.tag == "Player");
            //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        }
        //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
       // showSelectionCircleServerRpc(getRealName(unitToAdd), unitToAdd.tag == "Player");

    }

    string getRealName(GameObject go)
    {
        if (go.tag == "Player")
            return go.GetComponent<CombatController>().pg_name;
        else
            return go.name;
    }

    public void ShiftSelect(GameObject unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            showSelectionCircleServerRpc(getRealName(unitToAdd), unitToAdd.tag == "Player");
            //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        }
        /* else
        {
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitSelected.Remove(unitToAdd);
        } */
    }

    public void DragSelect(GameObject unitToAdd)
    {

    }
    public void DeselectAll()
    {

        foreach (var unit in unitSelected)
        {
            deselectServerRpc(getRealName(unit), unit.tag == "Player");
        }
        unitSelected.Clear();

    }

    [ServerRpc(RequireOwnership = false)]
    void deselectServerRpc(string name, bool isPlayer)
    {
        deselectClientRpc(name, isPlayer);
    }

    [ClientRpc]
    void deselectClientRpc(string name, bool isPlayer)
    {
        if (isPlayer)
        {
            GameObject[] pgs = GameObject.FindGameObjectsWithTag("Player");
            foreach (var pg in pgs)
            {
                Debug.Log(":::::::: DESELECT ::::::: "+name+" "+pg.GetComponent<CombatController>().pg_name);
                if (name == pg.GetComponent<CombatController>().pg_name)
                {
                    pg.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            GameObject.Find(name).transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void showSelectionCircleServerRpc(string name, bool isPlayer)
    {
        showSelectionCircleClientRpc(name, isPlayer);
    }

    [ClientRpc]
    void showSelectionCircleClientRpc(string name, bool isPlayer)
    {
        if (isPlayer)
        {
            GameObject[] pgs = GameObject.FindGameObjectsWithTag("Player");
            foreach (var pg in pgs)
            {
                if (name == pg.GetComponent<CombatController>().pg_name)
                {
                    pg.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            GameObject.Find(name).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}


