using UnityEngine;
using MLAPI;
using UnityEngine.UI;

public class CharacterImageController : NetworkBehaviour
{

    public GameObject player_image;
    // Start is called before the first frame update
    void Start()
    {
        if(IsLocalPlayer){
            player_image.GetComponent<Image>().sprite = Resources.Load("tokens/"+PlayerPrefs.GetInt("player_id")+"_s", typeof(Sprite)) as Sprite; 
        }
    }

}
