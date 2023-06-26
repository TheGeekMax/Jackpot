using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour{

    public GameObject rollButton;

    bool holding = false;

    [Header("RollButton Min & Max y pos")]
    public float minY = -4f;
    public float maxY = -7f;

    public static TouchManager instance;
    // Start is called before the first frame update
    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void Setup(){
        //On reffuse le multi touch
        Input.multiTouchEnabled = false;
    }

    void Update(){
        if(!JackpotManager.instance.finishedRolling){
            return;
        }
        //on regarde si on a touché l'ecran
        if(Input.touchCount > 0){
            //on regarde si c'est le debut du touch
            if(Input.GetTouch(0).phase == TouchPhase.Began){
                //Debug.Log("Began");
                //on regarde si on a touché le bouton par rapport a ses coordonnées (1 unit de largeur et 1 unit de hauteur)
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                if(touchPos.x > rollButton.transform.position.x - 0.5f && touchPos.x < rollButton.transform.position.x + 0.5f && touchPos.y > rollButton.transform.position.y - 0.5f && touchPos.y < rollButton.transform.position.y + 0.5f){
                    holding = true;
                }
            }else if(holding && Input.GetTouch(0).phase == TouchPhase.Moved){
                //Debug.Log("Moved");
                //on regarde si on a touché le bouton par rapport a ses coordonnées (1 unit de largeur et 1 unit de hauteur)
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Debug.Log(touchPos);
                //on change les coordonnées du bouton (seulement en y) puis on clamp 
                rollButton.transform.position = new Vector3(rollButton.transform.position.x, Mathf.Clamp(touchPos.y, maxY, minY),0);

                if(rollButton.transform.position.y <= maxY+0.01f){
                    //on le remet a sa position de base
                    rollButton.transform.position = new Vector3(rollButton.transform.position.x, minY,0);
                    holding = false;
                    JackpotManager.instance.Roll();
                }
            }else if(Input.GetTouch(0).phase == TouchPhase.Ended){
                //Debug.Log("Ended");
                //on remet le bouton a sa position de base
                rollButton.transform.position = new Vector3(rollButton.transform.position.x, minY,0);
                holding = false;
            }
        }
    }
}
