using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Animate : MonoBehaviour{
    Vector2 startPos;
    public TextMeshProUGUI text;

    float time=1f;
    public float speed = 1f;

    void Start(){
        startPos = transform.position;
        gameObject.SetActive(false);
    }

    void Update(){
        if(time < 1){
            time += Time.deltaTime * speed;
            //ici on met le pas a time^0.8
            transform.position = Vector2.Lerp(startPos, startPos + (new Vector2(0,100)), 1f-Mathf.Pow(1f-time, 3f));
            //on met la transparence a 1-time
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1-time);
            
        }else{
            //on le desactive
            gameObject.SetActive(false);
        }
    }

    public void Reset(string str){
        time = 0;
        transform.position = startPos;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.text = str;
    }
}
