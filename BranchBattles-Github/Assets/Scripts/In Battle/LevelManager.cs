using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool buttonStart;

    
    public GameObject StartScreen;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject BattleUI;

    public GameObject PlayerObject;
    public GameObject EnemyObject;

    public int currentLevel;
    public int currentTroop;

    //public int newTroopNumber;
    public Unit newTroop;

    public GameObject[] AllUnits;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonStart)
        {
            BattleUI.SetActive(false);

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
        BattleUI.SetActive(false);

        AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in AllUnits)
        {
            Unit Troop = unit.GetComponent<Unit>();
            if (Troop != null)
            {
                if (Troop.Team == losingTeam)
                {
                    Troop.Defeat();
                }
                else {
                    Troop.Victory();
                }
               
            }
        }

        if (losingTeam == -1) //Activates the correct UI 
        {
            VictoryScreen.SetActive(true);
            if (PlayerPrefs.GetInt("CompletedLevels") < currentLevel) {
                //Debug.Log("Next level unlocked:" + currentLevel);
                PlayerPrefs.SetInt("CompletedLevels", currentLevel);
            }

            if (PlayerPrefs.GetInt("UnlockedTroops") < currentTroop)
            {
                //Debug.Log("Adding new troop");
                PlayerPrefs.SetInt("UnlockedTroops", currentTroop);
                if (currentTroop < 5) {
                    PlayerInfo.PlayerTroops[currentTroop] = newTroop; 
                }
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

        //if (newTroop != null) {
          //  PlayerInfo.PlayerTroops[newTroopNumber] = newTroop;
        //}

        PlayerObject.SetActive(true);
        EnemyObject.SetActive(true);
        BattleUI.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMap()
    {
        Debug.Log("Returning to the map");
        SceneManager.LoadScene("LevelSelect");
    }
}
