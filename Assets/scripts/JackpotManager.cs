using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct winData{
    public int[,] wingrid;
    public int multiplier;
}

public class JackpotManager : MonoBehaviour{   

    public int maxNumb = 4;

    int[] roll1;
    int[] roll2;
    int[] roll3;

    float roll1Count;
    float roll2Count;
    float roll3Count;

    public bool finishedRolling = true;
    public bool finishedAnimating = true;

    public GameObject plusAnimationText;
    public GameObject minusAnimationText;

    public static JackpotManager instance;
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
        roll1 = new int[maxNumb];
        roll2 = new int[maxNumb];
        roll3 = new int[maxNumb];

        //on remplie les tableaux de 0 a 9
        for(int i = 0; i < maxNumb; i++){
            roll1[i] = i;
            roll2[i] = i;
            roll3[i] = i;
        }

        //on les melange
        for(int i = 0; i < maxNumb; i++){
            int rand = Random.Range(0, maxNumb);
            int temp = roll1[i];
            roll1[i] = roll1[rand];
            roll1[rand] = temp;
        }
        for(int i = 0; i < maxNumb; i++){
            int rand = Random.Range(0, maxNumb);
            int temp = roll2[i];
            roll2[i] = roll2[rand];
            roll2[rand] = temp;
        }
        for(int i = 0; i < maxNumb; i++){
            int rand = Random.Range(0, maxNumb);
            int temp = roll3[i];
            roll3[i] = roll3[rand];
            roll3[rand] = temp;
        }

        ViewManager.instance.Updates(roll1, roll2, roll3);
    }

    void Update(){
        if(!finishedRolling){
            if(roll1Count > 0){
                roll1Count-= Time.deltaTime;
                int temp = roll1[0];
                for(int i = 0; i < maxNumb - 1; i++){
                    roll1[i] = roll1[i + 1];
                }
                roll1[maxNumb - 1] = temp;
            }
            if(roll2Count > 0){
                roll2Count-= Time.deltaTime;
                int temp = roll2[0];
                for(int i = 0; i < maxNumb - 1; i++){
                    roll2[i] = roll2[i + 1];
                }
                roll2[maxNumb - 1] = temp;
            }
            if(roll3Count > 0){
                roll3Count-= Time.deltaTime;
                int temp = roll3[0];
                for(int i = 0; i < maxNumb - 1; i++){
                    roll3[i] = roll3[i + 1];
                }
                roll3[maxNumb - 1] = temp;
            }
            if(roll1Count <= 0 && roll2Count <= 0 && roll3Count <= 0){
                finishedRolling = true;
                //on execute le code pour voir si on a gagné (todo)
                //on recupere les données
                winData dt = CalculateWin();
                //on les envois a la view
                ViewManager.instance.Zoom(dt.wingrid);
                //on ajoute l'argent
                MoneyManager.instance.Play(dt.multiplier);

                MoneyManager.instance.UpdateBet(0);
                //on affiche le texte
                if(dt.multiplier > 0){
                    plusAnimationText.SetActive(true);
                    //on lance l'animation
                    plusAnimationText.GetComponent<Animate>().Reset("+ "+(ViewManager.instance.IntToAlphabetNotation(MoneyManager.instance.bet * dt.multiplier)));
                }
            }

            ViewManager.instance.Updates(roll1, roll2, roll3);
        }

        //on print les valeurs
        //Debug.Log("R1: " + roll1 + " R2: " + roll2 + " R3: " + roll3);
    }

    //calcul win
    public winData CalculateWin(){
        int[,] wingrid = new int[3, 3];
        int multiplier = 0;
        //etape 0, on recupere la grille d'etat
        for(int i = 0; i < 3; i ++){
            for(int j = 0; j < 3; j++){
                wingrid[i, j] = 0;
            }
        }
        //chaque collonnes represente un rouleau, la ligne du dessu a l'indice len-1 , milieu 0 et bas 1

        int[,] gridstate = new int[3, 3];
        gridstate[0, 2] = roll1[roll1.Length - 1];
        gridstate[0, 1] = roll1[0];
        gridstate[0, 0] = roll1[1];

        gridstate[1, 2] = roll2[roll2.Length - 1];
        gridstate[1, 1] = roll2[0];
        gridstate[1, 0] = roll2[1];

        gridstate[2, 2] = roll3[roll3.Length - 1];
        gridstate[2, 1] = roll3[0];
        gridstate[2, 0] = roll3[1];

        int winId = 1;

        //etape 1, on regarde si 3 identiques sur une ligne (ou 2)
        for(int i = 0; i < 3; i++){
            if(gridstate[0, i] == gridstate[1, i] && gridstate[1, i] == gridstate[2, i]){
                wingrid[0, i] = winId;
                wingrid[1, i] = winId;
                wingrid[2, i] = winId;

                winId++;

                multiplier += 5;
            }else if(gridstate[0, i] == gridstate[1, i]){
                wingrid[0, i] = winId;
                wingrid[1, i] = winId;

                winId++;

                multiplier += 2;
            }else if(gridstate[1, i] == gridstate[2, i]){
                wingrid[1, i] = winId;
                wingrid[2, i] = winId;

                winId++;

                multiplier += 2;
            }
        }

        //etape 2, on regarde les diagonales
        if(gridstate[0, 0] == gridstate[1, 1] && gridstate[1, 1] == gridstate[2, 2]){
            wingrid[0, 0] = winId;
            wingrid[1, 1] = winId;
            wingrid[2, 2] = winId;

            winId++;

            multiplier += 3;
        }

        if(gridstate[0, 2] == gridstate[1, 1] && gridstate[1, 1] == gridstate[2, 0]){
            wingrid[0, 2] = winId;
            wingrid[1, 1] = winId;
            wingrid[2, 0] = winId;

            winId++;

            multiplier += 3;
        }

        //on retourne les données
        winData data = new winData();
        data.wingrid = wingrid;
        data.multiplier = multiplier;
        return data;
    }

    public void Roll(){
        if(!finishedRolling || !MoneyManager.instance.CanPlay() || !finishedAnimating){
            return;
        }
        MoneyManager.instance.RemoveBet();
        //on affiche le text de minus
        minusAnimationText.SetActive(true);
        //on lance l'animation
        minusAnimationText.GetComponent<Animate>().Reset("- "+ViewManager.instance.IntToAlphabetNotation(MoneyManager.instance.bet));
        //float random entre 3 et 5
        roll1Count = Random.Range(1f, 3f);
        roll2Count = Random.Range(1f, 3f);
        roll3Count = Random.Range(1f, 3f);
        finishedRolling = false;
        finishedAnimating = false;
    }
}
