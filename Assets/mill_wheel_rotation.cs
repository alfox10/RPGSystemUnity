using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mill_wheel_rotation : MonoBehaviour
{

    public float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0.0f, 0.0f, Space.Self);
    }
}
