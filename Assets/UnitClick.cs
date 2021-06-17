using UnityEngine;
using MLAPI;

public class UnitClick : NetworkBehaviour
{

    private Camera cam;
    public LayerMask selectable;
    public LayerMask gui;
    void Start()
    {
        if(IsHost){
            cam = Camera.main;
        }
    }


    void Update()
    {
        if(IsHost){
            if(Input.GetMouseButtonDown(0)){
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, selectable)){
                    
                    if(Input.GetKey(KeyCode.LeftShift)){
                        UnitSelection.Instance.ShiftSelect(hit.collider.gameObject);
                    }else {
                        Debug.Log(hit.collider.name);
                        UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}
