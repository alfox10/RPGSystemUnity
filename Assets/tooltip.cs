using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltip : MonoBehaviour
{
    public float x_offset=0;
    public float y_offset=0;
    public Text text_content;
    public Image image_sprite;

    public Text header_text;
    public void SetText(string content, string image_content, string header){
        text_content.text = content;
        header_text.text = header;
        image_sprite.sprite = Resources.Load("item_icons/"+image_content, typeof(Sprite)) as Sprite;

    }

    private void Update(){
        Vector2 m_pos = Input.mousePosition;
        Vector2 offset = new Vector2((transform.GetComponent<RectTransform>().sizeDelta.x),(transform.GetComponent<RectTransform>().sizeDelta.y/2));
        transform.position = new Vector2(m_pos.x+offset.x,m_pos.y-offset.y);
    }
}
