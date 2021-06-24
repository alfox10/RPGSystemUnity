using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class CharacterUnit : NetworkBehaviour
{
    
    void Start()
    {
        AddNewUnitServerRpc();
         if(IsHost) {
            GameObject[] mobs = GameObject.FindGameObjectsWithTag("enemy");
            foreach (var mob in mobs){
                if(!UnitSelection.Instance.unitList.Contains(mob)){
                    UnitSelection.Instance.unitList.Add(mob);
                }
            }
        } 
    }

    
    private void OnDestroy() {
        RemoveNewUnitServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    public void AddNewUnitServerRpc(){
        AddNewUnitClientRpc();
    }

    [ClientRpc]
    public void AddNewUnitClientRpc(){
        if(!UnitSelection.Instance.unitList.Contains(this.gameObject))
            UnitSelection.Instance.unitList.Add(this.gameObject);
    }
    [ServerRpc(RequireOwnership = false)]
    public void RemoveNewUnitServerRpc(){
        RemoveNewUnitClientRpc();
    }

    [ClientRpc]
    public void RemoveNewUnitClientRpc(){
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
