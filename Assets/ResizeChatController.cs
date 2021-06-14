using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeChatController : MonoBehaviour
{
    public GameObject panel;
    public Sprite min;
    public Sprite max;
    private Button _buttonresize;
    private RectTransform paneltransform;
    private RectTransform _buttontransform;

    private Image _buttoncurrentimage;
    // Start is called before the first frame update
    void Start()
    {
        _buttonresize = GetComponent<Button>();
        _buttonresize.onClick.AddListener(resizeChat);
        _buttoncurrentimage = GetComponent<Image>();
        _buttontransform = GetComponent<RectTransform>();
        paneltransform =  panel.GetComponent<RectTransform>();
    }

    private void resizeChat(){
        if(paneltransform.sizeDelta.y < 100){
            paneltransform.sizeDelta = new Vector2(paneltransform.sizeDelta.x,400);
            _buttoncurrentimage.sprite = min; 
           // _buttontransform.position = new Vector2(_buttontransform.position.x,433);
        } else {
            paneltransform.sizeDelta = new Vector2(paneltransform.sizeDelta.x,100);
            _buttoncurrentimage.sprite = max; 
           // _buttontransform.position = new Vector2(_buttontransform.position.x,93);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
