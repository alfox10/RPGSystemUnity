using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanelController : MonoBehaviour
{   
    public Button[] buttons;
    public GameObject[] panels;

    public GameObject mainMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<buttons.Length;i++){
            int current_i_value = i;
            buttons[i].onClick.AddListener(delegate{openPanel(current_i_value);});
        }
    }

    void openPanel(int i){
        Debug.Log("i VALUE : "+i);
        panels[i].SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void turnBackToMainPanel(){
        foreach (var item in panels)
        {
            item.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
    }
  
}
