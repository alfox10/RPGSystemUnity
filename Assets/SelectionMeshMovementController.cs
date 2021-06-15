using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMeshMovementController : MonoBehaviour
{
    public float speed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f,0f,speed * Time.deltaTime, Space.Self);
        float s_scale = Mathf.Sin(Time.time);
        Vector3 vec = new Vector3(s_scale,s_scale, 1);
         transform.localScale = vec;
    }
}
