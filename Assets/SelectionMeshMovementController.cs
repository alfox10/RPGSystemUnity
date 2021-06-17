using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMeshMovementController : MonoBehaviour
{
    public float speed = 0f;

    public bool isAnim=false, isExitAnim=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnim)
            loopAnim();
        else
            setStaticMeshSize();
        
    }

    void loopAnim(){
        //Debug.Log("starting anim for : "+transform.parent.name);
        transform.Rotate(0f,0f,speed * Time.deltaTime, Space.Self);
        float s_scale = Mathf.Sin(Time.time);
        Vector3 vec = new Vector3(s_scale,s_scale, 1);
        transform.localScale = vec;
    }

    void setStaticMeshSize(){
       // Debug.Log("stopping anim for : "+transform.parent.name);
       // isAnim = false;
        transform.localScale = new Vector3(1f,1f,1f);
        isExitAnim = false;
    }
}
