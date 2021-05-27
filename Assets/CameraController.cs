using UnityEngine;
using MLAPI;

public class CameraController : NetworkBehaviour
{
    public Transform target;
    public float speed= 50f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate() {
      if(!IsLocalPlayer){
            transform.gameObject.SetActive(false);
            return;
        }
      if(IsLocalPlayer){
        Vector3 desiredPosition = target.position + offset;
        // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed*Time.deltaTime);  
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, speed*Time.deltaTime);
        transform.position = smoothedPosition;
      }
    }

}
