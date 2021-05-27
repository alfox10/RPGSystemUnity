using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltip : MonoBehaviour
{
    public Text text_content;
    public void SetText(string content){
        text_content.text = content;
    }

    private void Update(){
        Vector2 m_pos = Input.mousePosition;
        transform.position = m_pos;
    }
}
