using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewManager : MonoBehaviour{
    public Sprite[] sprites;

    public ViewComponent[] viewComponentsTop;
    public ViewComponent[] viewComponentsCenter;
    public ViewComponent[] viewComponentsBottom;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI betText;

    public TextMeshProUGUI bonusTimerText;

    public static ViewManager instance;

    public LedComponent led1;
    public LedComponent led2;
    public LedComponent led3;
    public LedComponent led4;

    float ledCicleTime = 0f;

    public int[,] wintableState;
    int zoomId = 0;
    // Start is called before the first frame update
    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void Setup(){
        //On setup comme si c'etait un start
        wintableState = new int[3,3];
        for(int i = 0; i < wintableState.GetLength(0); i++){
            for(int j = 0; j < wintableState.GetLength(1); j++){
                wintableState[i,j] = 0;
            }
        }
    }

    public void Update(){
        //on s'occume du timer de bonus format mm:ss:ms 
        long time = (long)Mathf.Max(0,MoneyManager.instance.timeSinceLastBonus - System.DateTime.Now.Ticks);
        System.TimeSpan ts = System.TimeSpan.FromTicks(time);
        bonusTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        //on s'occupe de l'animation des leds
        switch(ledCicleTime){
            case < 0.2f:
                led1.TurnOn();
                break;
            
            case < 0.4f:
                led2.TurnOn();
                break;

            case < 0.6f:
                led3.TurnOn();
                break;
            
            case < 0.8f:
                led4.TurnOn();
                break;
        }

        if(ledCicleTime > 1f){
            ledCicleTime = 0f;
        }else{
            ledCicleTime += Time.deltaTime;
        }
    }


    public void Zoom(int[,] winTable){
        wintableState = winTable;
        //wintableState = new int[3,3]{{1,2,3}, {4,5,6}, {7,8,9}};
        zoomId = 1;
        
        //On fait l'effet de zoom ssi winTable[i,j] == 1
        ZoomAux();
    }

    public bool Exist(int value){
        for(int i = 0; i < wintableState.GetLength(0); i++){
            for(int j = 0; j < wintableState.GetLength(1); j++){
                if(wintableState[i,j] == value){
                    return true;
                }
            }
        }
        return false;
    }

    public void ZoomAux(){
        if(!Exist(zoomId)){
            JackpotManager.instance.finishedAnimating = true;
            return;
        }

        for(int i = 0; i < wintableState.GetLength(0); i++){
            if(wintableState[i, 0] == zoomId){
                viewComponentsTop[i].Animate();
            }
        }
        //ligne du centre
        for(int i = 0; i < wintableState.GetLength(0); i++){
            if(wintableState[i,1] == zoomId){
                viewComponentsCenter[i].Animate();
            }
        }
        //ligne du bas
        for(int i = 0; i < wintableState.GetLength(0); i++){
            if(wintableState[i,2] == zoomId){
                viewComponentsBottom[i].Animate();
            }
        }

        zoomId++;
        Invoke("ZoomAux", 0.5f);
    }

    public void Updates(int[] roll1, int[] roll2, int[] roll3){
        viewComponentsCenter[0].SetSprite(sprites[roll1[0]]);
        viewComponentsCenter[1].SetSprite(sprites[roll2[0]]);
        viewComponentsCenter[2].SetSprite(sprites[roll3[0]]);

        //top
        viewComponentsTop[0].SetSprite(sprites[roll1[1]]);
        viewComponentsTop[1].SetSprite(sprites[roll2[1]]);
        viewComponentsTop[2].SetSprite(sprites[roll3[1]]);

        //bottom (on prend le dernier de roll)
        viewComponentsBottom[0].SetSprite(sprites[roll1[roll1.Length - 1]]);
        viewComponentsBottom[1].SetSprite(sprites[roll2[roll2.Length - 1]]);
        viewComponentsBottom[2].SetSprite(sprites[roll3[roll3.Length - 1]]);

    }

    public string IntToAlphabetNotation(int value){
        //on cherche la puissance de 1000 la plus grande
        int power = 0;
        while(value > Mathf.Pow(1000, power)){
            power++;
        }
        if(value < 1000){
            return value.ToString();
        }

        //on renvoie en tronquant la valeur
        string[] powerValue = new string[27] {"", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
        return (value / Mathf.Pow(1000, power - 1)).ToString() + " " +powerValue[Mathf.Max(0,power-1)];
    }

    public void UpdateMoney(int money){
        moneyText.text = "$ "+IntToAlphabetNotation(money);
    }

    public void UpdateBet(int bet){
        betText.text = "bet: " + IntToAlphabetNotation(bet);
    }
}
