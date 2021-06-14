using UnityEngine;
using UnityEngine.UI;
using MLAPI;
public class InventoryStatsController : NetworkBehaviour
{

    public GameObject uiManager;
    public Text hp;
    public Text mana;
    public Text pgName; 
    public Text pgClass;
    public Image class_type;
    public Text level;
    public Image exp_line;
    public int max_level_exp;




    private string mana_type;
    private bool isAlreadySet;
    

    void Start()
    {
        if(IsLocalPlayer){
            setGame();
            isAlreadySet = false;
            max_level_exp = 1000;
        }
    }

    void Update() {
        if(IsLocalPlayer){
            if(!isAlreadySet && uiManager.GetComponent<UIManager>().pgStats != null){
                isAlreadySet = true;
                UIManager uIManager = uiManager.GetComponent<UIManager>();
                hp.text = uIManager.pgStats.max_hp+" hp";
                mana.text = uIManager.pgStats.max_omens+" "+mana_type;
                pgName.text = uIManager.pgStats.name;
                pgClass.text = uIManager.pgStats.pg_class;
                class_type.sprite = Resources.Load("ui_v3/class_icon_"+uIManager.pgStats.class_type, typeof(Sprite)) as Sprite;
                exp_line.fillAmount = ((float)uIManager.pgStats.exp/(float)max_level_exp);
                level.text = uIManager.pgStats.level.ToString();

            }
        }
    }

    void setGame(){
        mana_type = "";
         if(PlayerPrefs.GetString("game") == "MorkBorg")
            mana_type = "omens";
        else
            mana_type = "mana";
    }
}
