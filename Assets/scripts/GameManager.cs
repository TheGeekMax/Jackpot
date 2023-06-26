using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //on setup l'instance
    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Start(){
        ViewManager.instance.Setup();
        JackpotManager.instance.Setup();
        MoneyManager.instance.Setup();

        TouchManager.instance.Setup();
    }
}
