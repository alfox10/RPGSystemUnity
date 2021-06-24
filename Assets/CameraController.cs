using UnityEngine;
using MLAPI;

public class CameraController : NetworkBehaviour
{
    public Transform target;
    public float maxZRange=8f;
    public float maxXRange=30f;
    public float maxXRange_UP=8f;
    public bool isTeleporting=false;
    public float deltaPos=10f;
    public float speed= 50f;
    public float cameraMouseSpeed=10f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;


    void LateUpdate() {
      if(!IsLocalPlayer){
            transform.gameObject.SetActive(false);
            return;
        }
      if(IsLocalPlayer){
        if(isTeleporting){
          transform.position = target.position+offset;
          isTeleporting = false;
          return;
        }
        float axixsMovement = Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"));
        if(axixsMovement > 0f){
          Vector3 desiredPosition = target.position + offset;
          // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed*Time.deltaTime);  
          Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, speed*Time.deltaTime);
          transform.position = smoothedPosition;
        } else {
          float target_dist_z =  transform.position.z;
          float target_dist_x = transform.position.x;


          if (Input.mousePosition.x < deltaPos && target_dist_z > target.position.z-maxZRange){// && target_dist_z_plus < cam_z_abs_pos){

              transform.position +=  new Vector3(0f,0f,-1f) * Time.deltaTime * cameraMouseSpeed;
          }
          if (Input.mousePosition.x > (Screen.width - deltaPos) && target_dist_z < target.position.z+maxZRange){

              transform.position +=  new Vector3(0f,0f,1f) * Time.deltaTime * cameraMouseSpeed;
          }
          if (Input.mousePosition.y < deltaPos  && target_dist_x < target.position.x+maxXRange){
              transform.position +=  new Vector3(1f,0f,0f) * Time.deltaTime * cameraMouseSpeed;
          }
          if (Input.mousePosition.y > (Screen.height - deltaPos) && target_dist_x > target.position.x){
              transform.position +=  new Vector3(-1f,0f,0f) * Time.deltaTime * cameraMouseSpeed;
          }
        }
      }
    }

}
