using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;

public class UIManager : NetworkBehaviour
{
    [Header("Game Object imported")]
    public GameObject dice;
    public GameObject dice_canvas;
    public GameObject inventory_canvas;
    public GameObject gm_canvas;
    public GameObject roll_button_obj;
    public GameObject inventory_button_obj;
    public GameObject close_button_obj;
    public GameObject open_button_obj;

    [Header("Dice Buttons")]
    public Button d20;
    public Button d4;
    public Button d6;
    public Button d8;
    public Button d10;
    public Button d12;
    public GameObject diceGameCtl;
    private DiceGameController dgc;

    [Header("GUI")]
    public Text name_gui;
    public Text hp_gui;
    public Text omens_gui;
    public GameObject gm_obj;
    public Button add_hp;
    public Button remove_hp;
    public Button add_omens;
    public Button remove_omens;

    [Header("Movement Dice")]
    public Text chat_text;
    public float roll_canvass_speed;
    public float __INIT__SPEED__SCALE;
    public float __INIT__SMOOTHING;    
    public float __INIT__SMOOTH__DIVIDER;
    //private
    private Button gm;
    private PGstats pgStats;
    private float speed_scale;
    private float smoothing;
    private int idx;
    private Image im;
    private int player_id;
    private Button roll_button;
    private Button close_button;
    private Button open_button;
    private Button open_close_inventory;
    private float smoth_divider;
    private bool isRolling = false;
    private int minDiceFace;
    private int maxDiceFace;
    private int diceResult;
    private string diceNameFormat;
    private List<DiceFormat> diceSwitchHandler;
    private string gameName;
    private string mana_type;

    public class DiceFormat{
        public int initialSprite;
        public int minRange;
        public int maxRange;
        public string diceName;
    }


    void setInitialDice(){
        im.sprite = dgc.GetSprite(0);
        minDiceFace = 0;
        maxDiceFace = 20;
        diceNameFormat = "d20";

        diceSwitchHandler = new List<DiceFormat>();
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d20").startImage, minRange=dgc.GetDiceInfo("d20").startImage, maxRange=dgc.GetDiceInfo("d20").endImage, diceName="d20"});
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d4").startImage, minRange=dgc.GetDiceInfo("d4").startImage, maxRange=dgc.GetDiceInfo("d4").endImage, diceName="d4"});
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d6").startImage, minRange=dgc.GetDiceInfo("d6").startImage, maxRange=dgc.GetDiceInfo("d6").endImage, diceName="d6"});
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d8").startImage, minRange=dgc.GetDiceInfo("d8").startImage, maxRange=dgc.GetDiceInfo("d8").endImage, diceName="d8"});
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d10").startImage, minRange=dgc.GetDiceInfo("d10").startImage, maxRange=dgc.GetDiceInfo("d10").endImage, diceName="d10"});
        diceSwitchHandler.Add(new DiceFormat{initialSprite=dgc.GetDiceInfo("d12").startImage, minRange=dgc.GetDiceInfo("d12").startImage, maxRange=dgc.GetDiceInfo("d12").endImage, diceName="d12"});

    }

    void Start()
    {
        if(IsLocalPlayer){
            gameName = PlayerPrefs.GetString("game");
            setManaType();
            diceGameCtl = GameObject.Find("DiceImageController");
            chat_text = GameObject.Find("chat_text").GetComponent<Text>();
            dgc = diceGameCtl.GetComponent<DiceGameController>();
            //test.sprite = Resources.Load("item_icons/rings", typeof(Sprite)) as Sprite;
            player_id = PlayerPrefs.GetInt("player_id");
            diceResult = 0;
            idx = 0;
            im = dice.GetComponent<Image>();
            setInitialDice();
            roll_button = roll_button_obj.GetComponent<Button>();
            roll_button.onClick.AddListener(rollOnButtonClick);
            if(IsHost){
                gm = gm_obj.GetComponent<Button>();
                gm.onClick.AddListener(openCloseGMCommands);
            } else {
                gm_obj.transform.gameObject.SetActive(false);
            }
            close_button = close_button_obj.GetComponent<Button>();
            close_button.onClick.AddListener(closeRoll);
            open_button = open_button_obj.GetComponent<Button>();
            open_button.onClick.AddListener(openRoll);
            open_close_inventory = inventory_button_obj.GetComponent<Button>();
            open_close_inventory.onClick.AddListener(openCloseInventory);
            d20.onClick.AddListener(delegate{changeDice(0);});
            d4.onClick.AddListener(delegate{changeDice(1);});
            d6.onClick.AddListener(delegate{changeDice(2);});
            d8.onClick.AddListener(delegate{changeDice(3);});
            d10.onClick.AddListener(delegate{changeDice(4);});
            d12.onClick.AddListener(delegate{changeDice(5);});

            speed_scale = __INIT__SPEED__SCALE;
            smoothing = __INIT__SMOOTHING;
            smoth_divider = __INIT__SMOOTH__DIVIDER;
            //TODO test only, sostiture 5 con variabile
            Debug.Log("Init Player");
            initPG(player_id);
        }
    }

    public class PGid{
        public int id;
    }

    public class PGstats{
        //hp, max_hp, omens, name
        public int hp;
        public int max_hp;
        public int omens;
        public string name;
    }

    void setManaType(){
        if(gameName == "MorkBorg")
            mana_type = "Omens";
        else
            mana_type = "Mana";
    }

    void openCloseGMCommands(){
        if(gm_canvas.activeSelf){
            gm_canvas.SetActive(false);
        }else{
            gm_canvas.SetActive(true);
        }
    }
    void initPG(int p_id){
        StartCoroutine(setStatsPG(p_id));
        add_hp.onClick.AddListener(delegate{HPChanger(0);});
        remove_hp.onClick.AddListener(delegate{HPChanger(1);});
        add_omens.onClick.AddListener(delegate{HPChanger(2);});
        remove_omens.onClick.AddListener(delegate{HPChanger(3);});
    }

    void HPChanger(int change){
        if(change == 0){
            if(pgStats.hp < pgStats.max_hp){
                pgStats.hp +=1;
            }
        }else if(change == 1){
            if(pgStats.hp > 0){
                pgStats.hp -=1;
            }
        }else if(change == 2){
            pgStats.omens +=1;
        }else if(change == 3){
            if(pgStats.omens > 0){
                pgStats.omens -=1;
            }
        }else{
            Debug.Log("Error cannot recognize button HP");
        }

        changeGuiStatInfo(pgStats);

    }

    IEnumerator setStatsPG(int p_id){
        PGid jsonbody = new PGid();
        jsonbody.id = p_id;
        string json = JsonUtility.ToJson(jsonbody);
        var uwr = new UnityWebRequest("https://rpgsystem.alfox10.repl.co/api/v1/stats", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError){
            Debug.Log("Error While Sending: " + uwr.error);
        } else {
            Debug.Log(uwr.downloadHandler.text);
            pgStats = new PGstats();
            pgStats = JsonUtility.FromJson<PGstats>(uwr.downloadHandler.text);
            changeGuiStatInfo(pgStats);
        }
    }

    void changeGuiStatInfo(PGstats pgStats){
        hp_gui.text = "HP: "+pgStats.hp+"/"+pgStats.max_hp;
        omens_gui.text = mana_type+" : "+pgStats.omens;
        name_gui.text = pgStats.name;
    }

    void changeDice(int dice){
        im.sprite = dgc.GetSprite(diceSwitchHandler[dice].initialSprite);
        minDiceFace = diceSwitchHandler[dice].minRange;
        maxDiceFace = diceSwitchHandler[dice].maxRange;
        diceNameFormat = diceSwitchHandler[dice].diceName;
    }

    void closeRoll(){
        dice_canvas.SetActive(false);
    }

    void openCloseInventory(){
        if(inventory_canvas.activeSelf){
            inventory_canvas.SetActive(false);
        }else{
            inventory_canvas.SetActive(true);
        }
    }

    void openRoll(){
        dice_canvas.SetActive(true);
    }

    void rollOnButtonClick(){
        isRolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")){
            isRolling = true;
            
        }else if(isRolling){
            rollDice();
        }

    }

    void rollDice(){
        if(speed_scale > 0){
             dice.transform.localScale = Vector3.Lerp (dice.transform.localScale, Vector3.one*Random.Range(0.1f,0.3f), speed_scale);
             speed_scale -= Time.deltaTime*3;
        }
        
        
        if(smoothing > 0){
            float targetX = Random.Range(-80f,80f);
            float targetY = Random.Range(-80f,80f);
            Vector3 vv = new Vector3(targetX, targetY, 0f);
            dice.transform.localPosition = Vector3.Lerp(dice.transform.localPosition, vv, smoothing/smoth_divider);
            dice.transform.Rotate( Vector3.forward* Time.deltaTime * 300 * smoothing);
            smoothing -= Time.deltaTime;
            smoth_divider += Time.deltaTime*5;
            
            idx = Random.Range(minDiceFace,maxDiceFace);
            im.sprite = dgc.GetSprite(idx);
            

        }

        if(smoothing < 0 && speed_scale < 0){
            diceResult = (idx+1) - minDiceFace;
            print("STOP ROLLED with result: "+ diceResult);
            chat_text.text= chat_text.text+"\n"+pgStats.name+" rolled "+diceResult+" on a "+diceNameFormat;
            isRolling = false;
            speed_scale = __INIT__SPEED__SCALE;
            smoothing = __INIT__SMOOTHING;
            smoth_divider = __INIT__SMOOTH__DIVIDER;
            dice.transform.localScale = Vector3.Lerp (dice.transform.localScale, Vector3.one, 0.1f);
        }
    }
}