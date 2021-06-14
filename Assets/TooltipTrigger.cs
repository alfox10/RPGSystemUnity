using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string content;
    public string image_content;
    public string header;
    public bool isUsed=false;

    public void OnPointerEnter(PointerEventData eventData){
        TooltipSystem.Show(content, image_content, header, isUsed);
    }

    public void OnPointerExit(PointerEventData eventData){
        TooltipSystem.Hide();
    }
}
