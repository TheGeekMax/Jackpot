using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAnimation : MonoBehaviour
{   
    public static float lerp(float a, float b, float t){
        return a + (b - a) * t;
    }

    float time = 1f;

    // Update is called once per frame
    void Update(){
        if(time >= 1f){
            //on met le zoom a 1 
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            return;
        }
        if(time < 0.5f){
            //on zoom de 1 a 2
            float t = time / 0.5f;
            float scale = lerp(0.8f, 1.6f, t);
            transform.localScale = new Vector3(scale, scale, 1f);
        }else{
            //on dezoom de 2 a 1
            float t = (time - 0.5f) / 0.5f;
            float scale = lerp(1.6f, 0.8f, t);
            transform.localScale = new Vector3(scale, scale, 1f);
        }

        time += Time.deltaTime*2f;
    }


    public void BeginZoom(){
        time = 0f;
    }
}
