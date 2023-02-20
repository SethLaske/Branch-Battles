using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool buttonStart;
    public GameObject StartScreen;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject UI;

    public GameObject PlayerObject;
    public GameObject EnemyObject;

    public int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonStart)
        {
            UI.SetActive(false);

            PlayerObject.SetActive(false);
            EnemyObject.SetActive(false);


            StartScreen.SetActive(true);
        }
        else {  //Im assuming for things without a button start the scenes will be appropriately set up already
            //StartLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(int losingTeam) {
        UI.SetActive(false);
        if (losingTeam == -1) //Activates the correct UI 
        {
            VictoryScreen.SetActive(true);
            if (PlayerPrefs.GetInt("CompletedLevels") < currentLevel) {
                PlayerPrefs.SetInt("CompletedLevels", currentLevel);
            }
            
        }
        else if (losingTeam == 1)
        {
            DefeatScreen.SetActive(true);
            //PlayerPrefs.SetInt("CompletedLevels", 30);
        }
    }

    public void StartLevel() {
        StartScreen.SetActive(false);

        PlayerObject.SetActive(true);
        EnemyObject.SetActive(true);
        UI.SetActive(true);
    }

    
}
