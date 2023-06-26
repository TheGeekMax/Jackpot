using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedComponent : MonoBehaviour{
    public Sprite spriteOn;
    public Sprite spriteOff;

    public float duration = 0.4f;

    public float time = 0f;

    // Update is called once per frame
    void Update(){
        if(time > duration){
            GetComponent<SpriteRenderer>().sprite = spriteOff;
        }else{
            time += Time.deltaTime;
        }
    }

    public void TurnOn(){
        GetComponent<SpriteRenderer>().sprite = spriteOn;
        time = 0f;
    }
}
