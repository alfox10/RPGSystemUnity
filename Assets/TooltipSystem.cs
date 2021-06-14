using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public tooltip tooltip;
    public void Awake() {
        current = this;
    }

    public static void Show(string content, string image_content, string header, bool isUsed){
        if(isUsed){
            current.tooltip.SetText(content, image_content, header);
            current.tooltip.gameObject.SetActive(true);
        }
    }
    public static void Hide(){
        current.tooltip.gameObject.SetActive(false);
    }
}
