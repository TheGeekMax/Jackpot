using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int money = 1000;
    public int bet = 100;

    public long timeSinceLastBonus = 0;

    public GameObject buttonDailyBonus;

    public bool cheat = false;

    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public void Setup(){
        //On setup comme si c'etait un start
        money = PlayerPrefs.GetInt("MONEY", 1000);
        bet = PlayerPrefs.GetInt("BET", 100);

        ViewManager.instance.UpdateMoney(money);
        ViewManager.instance.UpdateBet(bet);
        //on le definit pour l'instant a l'heure actuelle sous forme d'un timestamp (depuis le 1er janvier 1970)
        //utilise les player pref d'un str vers int
        string tmBon = PlayerPrefs.GetString("BONUS_TIME", System.DateTime.Now.Ticks.ToString());
        timeSinceLastBonus = long.Parse(tmBon);
        //System.DateTime.Now.Ticks;

        Debug.Log(money);
    }

    public void Update(){
        //on verifie si on a depasse le temps pour le bonus
        if(timeSinceLastBonus < System.DateTime.Now.Ticks){
            //on active le bouton
            buttonDailyBonus.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }else{
            //on desactive le bouton
            buttonDailyBonus.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void AddDailyBonus(){
        long currentTime = System.DateTime.Now.Ticks;
        //on verifie que currentTime > timeSinceLastBonus
        if(currentTime < timeSinceLastBonus){
            Debug.Log("tick remaining : " + (timeSinceLastBonus - currentTime));
            return;
        }
        //on met timeSinceLastBonus a currentTime + 10 minutes
        timeSinceLastBonus = currentTime + 6000000000;
        money += 1000;

        ViewManager.instance.UpdateMoney(money);
        UpdateBet(0);
        //on sauvegarde le temps
        PlayerPrefs.SetString("BONUS_TIME", timeSinceLastBonus.ToString());
    }

    public void RemoveBet(){
        money -= bet;

        ViewManager.instance.UpdateMoney(money);
    }

    public void Play(int multiplier){
        money += bet * multiplier;

        PlayerPrefs.SetInt("MONEY", money);

        ViewManager.instance.UpdateMoney(money);
    }

    public void UpdateBet(int newBet){
        if(cheat){
            //on ajoute cette argent
            money += newBet*10;
            //on update la view
            ViewManager.instance.UpdateMoney(money);
            //on sauvegarde
            PlayerPrefs.SetInt("MONEY", money);

            return;
        }
        //definisson n = la puissance de 10000 la plus proche de money
        int n = 1;
        while(Mathf.Pow(1000,n) < money){
            n++;
        }
        newBet *= (int)Mathf.Pow(100,n-1);
        if(money == 0){
            bet = 0;
            ViewManager.instance.UpdateBet(bet);
            PlayerPrefs.SetInt("BET", bet);
            return;
        }
        //on verifie que le nouveau bet est inferieur a l'argent qu'on a
        if(newBet+bet > money){
            Debug.Log("Not enough money");
            bet = money;
        }else if(newBet+bet < 1){
            bet = 1;
        }else{
            bet += newBet;
        }
        PlayerPrefs.SetInt("BET", bet);
    
        ViewManager.instance.UpdateBet(bet);
    }

    public void MaxBet(){
        bet = money;

        ViewManager.instance.UpdateBet(bet);
    }

    public bool CanPlay(){
        return money >= bet;
    }
}
