using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;
using System;

public class UIManager : NetworkBehaviour
{
    [Header("Game Object imported")]
    public GameObject dice;
    public GameObject dice_canvas;
    public GameObject inventory_canvas;
    public GameObject gm_canvas;
    public GameObject combat_canvas;

    public Button quit_game;
    public Button open_close_combat;
    public GameObject roll_button_obj;
    public GameObject inventory_button_obj;

    public Button inventory_close_button;
   // public GameObject close_button_obj;
    public GameObject open_roll_button;

    [Header("Dice Buttons")]
    public Button d20;
    public Button d4;
    public Button d6;
    public Button d8;
    public Button d10;
    public Button d12;

    public GameObject[] dice_selection;
    public GameObject diceGameCtl;
    private DiceGameController dgc;

    [Header("GUI")]
    //public Text name_gui;
    public Text hp_gui;
    public Text omens_gui;
    public GameObject gm_obj;
    public Button add_hp;
    public Button remove_hp;
    public Button add_omens;
    public Button remove_omens;
    public Image hp_primary_color;
    public Image mana_primary_color;

    [Header("Movement Dice")]
    public Text chat_text;
    public float roll_canvass_speed;
    public float __INIT__SPEED__SCALE;
    public float __INIT__SMOOTHING;    
    public float __INIT__SMOOTH__DIVIDER;
    public PGstats pgStats;


    //private
    private Button gm;

    private float speed_scale;
    private float smoothing;
    private int idx;
    private Image im;
    private int player_id;
    private Button roll_button;
    //private Button close_button;
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


    private Image npc_image;

    public class DiceFormat{
        public int initialSprite;
        public int minRange;
        public int maxRange;
        public string diceName;
    }

    public PGstats getPlayerPrimaryInfo(){
        return pgStats;
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
            dgc = diceGameCtl.GetComponent<DiceGameController>();
            //test.sprite = Resources.Load("item_icons/rings", typeof(Sprite)) as Sprite;
            player_id = PlayerPrefs.GetInt("player_id");
            diceResult = 0;
            idx = 0;
            im = dice.GetComponent<Image>();
            setInitialDice();
            roll_button = roll_button_obj.GetComponent<Button>();
            roll_button.onClick.AddListener(rollOnButtonClick);
            quit_game.onClick.AddListener(doExitGame);
            open_close_combat.onClick.AddListener(openCloseCombat);
            inventory_close_button.onClick.AddListener(closeInvMenu);
            if(IsHost){
                gm = gm_obj.GetComponent<Button>();
                gm.onClick.AddListener(openCloseGMCommands);
            } else {
                gm_obj.transform.gameObject.SetActive(false);
                GameObject[] _onlyGM = GameObject.FindGameObjectsWithTag("npc_canvas_only_gm");
                foreach(GameObject _oGM in _onlyGM){
                    _oGM.transform.gameObject.SetActive(false);
                }
            }
           // close_button = close_button_obj.GetComponent<Button>();
           // close_button.onClick.AddListener(closeRoll);
            open_button = open_roll_button.GetComponent<Button>();
            open_button.onClick.AddListener(openCloseRollTable);
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
        public int max_omens;
        public string name;
        public string pg_class;
        public string class_type;
        public int level;
        public int exp;
    }

    void setManaType(){
        if(gameName == "MorkBorg")
            mana_type = "Omens";
        else
            mana_type = "Mana";
    }

    void openCloseCombat(){
        if(combat_canvas.activeSelf){
            combat_canvas.SetActive(false);
        }else{
            combat_canvas.SetActive(true);
        }
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
        string message="";
        if(change == 0){
            if(pgStats.hp < pgStats.max_hp){
                pgStats.hp +=1;
                message = "earned 1 HP";
            }
        }else if(change == 1){
            if(pgStats.hp > 0){
                pgStats.hp -=1;
                message = "lost 1 HP";
            } else {
                message = "IS DEAD";
            }
        }else if(change == 2){
            if(pgStats.omens < pgStats.max_omens){
                pgStats.omens +=1;
                message = "earned 1 "+mana_type;
            }
        }else if(change == 3){
            if(pgStats.omens > 0){
                pgStats.omens -=1;
                message = "lost 1 "+mana_type;
            }
        }else{
            Debug.Log("Error cannot recognize button HP");
            message = "error";
        }
        changeGuiStatInfo(pgStats);
        if(message != ""){
            message = "\n"+pgStats.name+" "+message;
            writeNewInfoOnChatServerRpc(message);
        }

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
        hp_gui.text = pgStats.hp+"/"+pgStats.max_hp;
        omens_gui.text = pgStats.omens+"/"+pgStats.max_omens;
        if(pgStats.hp == 0){
            hp_primary_color.fillAmount = 0;
        }else{
            hp_primary_color.fillAmount = ((float)pgStats.hp / (float)pgStats.max_hp);
        }

        if(pgStats.omens == 0){
            mana_primary_color.fillAmount = 0;
        }else{
            mana_primary_color.fillAmount = ((float)pgStats.omens / (float)pgStats.max_omens);
        }
    }

    void changeDice(int dice){
        if(!isRolling){
            im.sprite = dgc.GetSprite(diceSwitchHandler[dice].initialSprite);
            minDiceFace = diceSwitchHandler[dice].minRange;
            maxDiceFace = diceSwitchHandler[dice].maxRange;
            diceNameFormat = diceSwitchHandler[dice].diceName;
            foreach (var item in dice_selection)
            {
                item.SetActive(false);
            }
            dice_selection[dice].SetActive(true);
        }

    }


    void openCloseInventory(){
        if(inventory_canvas.activeSelf){
            inventory_canvas.SetActive(false);
        }else{
            inventory_canvas.SetActive(true);
        }
    }

    void closeInvMenu(){
        inventory_canvas.SetActive(false);
    }

    void openCloseRollTable(){
        if(dice_canvas.activeSelf){
            dice_canvas.SetActive(false);
        }else{
            dice_canvas.SetActive(true);
        }
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

    void doExitGame() {
        Application.Quit();
    }

    void rollDice(){
        if(speed_scale > 0){
             dice.transform.localScale = Vector3.Lerp (dice.transform.localScale, Vector3.one* UnityEngine.Random.Range(0.1f,0.3f), speed_scale);
             speed_scale -= Time.deltaTime*3;
        }
        
        
        if(smoothing > 0){
            float targetX = UnityEngine.Random.Range(-200f,200f);
            float targetY = UnityEngine.Random.Range(-200f,200f);
            Vector3 vv = new Vector3(targetX, targetY, 0f);
            dice.transform.localPosition = Vector3.Lerp(dice.transform.localPosition, vv, smoothing/smoth_divider);
            dice.transform.Rotate( Vector3.forward* Time.deltaTime * 300 * smoothing);
            smoothing -= Time.deltaTime;
            smoth_divider += Time.deltaTime*5;
            
            idx = UnityEngine.Random.Range(minDiceFace,maxDiceFace);
            im.sprite = dgc.GetSprite(idx);
            

        }

        if(smoothing < 0 && speed_scale < 0){
            diceResult = (idx+1) - minDiceFace;
            print("STOP ROLLED with result: "+ diceResult);
            setChatResultServerRpc("\n"+pgStats.name+" rolled "+diceResult+" on a "+diceNameFormat);
            isRolling = false;
            speed_scale = __INIT__SPEED__SCALE;
            smoothing = __INIT__SMOOTHING;
            smoth_divider = __INIT__SMOOTH__DIVIDER;
            dice.transform.localScale = Vector3.Lerp (dice.transform.localScale, Vector3.one, 0.1f);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void setChatResultServerRpc(string text_to_add){

        setChatResultClientRpc(text_to_add);
    }

    [ClientRpc]
    public void setChatResultClientRpc(string text_to_add){

        chat_text = GameObject.FindGameObjectWithTag("chat").GetComponent<Text>();
        chat_text.text = chat_text.text + text_to_add;
    }


    //dice_holder
    [ServerRpc(RequireOwnership = false)]
    public void setCombatDieSpriteServerRpc(int d_face, int npc_id){
        setCombatDieSpriteClientRpc(d_face, npc_id);
    }

    [ClientRpc]
    public void setCombatDieSpriteClientRpc(int d_face, int npc_id){

        GameObject[] npcs = GameObject.FindGameObjectsWithTag("dice_holder");

        foreach(GameObject npc in npcs){
            int local_npc_id = npc.GetComponent<NPCRollController>().npc_id;
            if(npc_id == local_npc_id){
                npc_image = npc.GetComponent<Image>();
                if(npc_image == null){
                    Debug.Log("no image found");
                } else {
                    diceGameCtl = GameObject.FindWithTag("DiceImageController");
                    dgc = diceGameCtl.GetComponent<DiceGameController>();
                    Sprite s = dgc.GetSprite(d_face);
                    npc_image.sprite = s;
                }              
            }
        }


    }


    [ServerRpc]
    void writeNewInfoOnChatServerRpc(string message){
        writeNewInfoOnChatClientRpc(message);
    }


    [ClientRpc]
    void writeNewInfoOnChatClientRpc(string message){
        chat_text = GameObject.FindGameObjectWithTag("chat").GetComponent<Text>();
        chat_text.text = chat_text.text + message;
    }
}
