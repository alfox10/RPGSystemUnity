using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    private static UnitSelection _instance;
    public static UnitSelection Instance{get{return _instance;}}


    void Awake(){
        if(_instance == null){
            _instance = this;
        }
    }


    public void ClickSelect(GameObject unitToAdd){
        DeselectAll();
        unitSelected.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShiftSelect(GameObject unitToAdd){
        if(!unitSelected.Contains(unitToAdd)){
            unitSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        } else {
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitSelected.Remove(unitToAdd);
        }
    }

    public void DragSelect(GameObject unitToAdd){
        
    }
    public void DeselectAll(){
        foreach (var unit in unitSelected)
        {
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        unitSelected.Clear();
    }

    public void Deselect (GameObject unitToDeselect){

    }
    
}


