using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Level manager ends up in the UI and deals with managing the level
public class LevelManager : MonoBehaviour
{
    public bool buttonStart;    //Allows me to choose if I want to press a button to start level

    //public static bool paused = true;   //Could also switch it to semaphore interpretation. Going to hold off for now, but makes sense to have a static variable controlling paused or not

    public GameObject StartScreen;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject BattleUI;
    public GameObject BattleUIUnits;

    public GameObject PlayerObject;
    public GameObject EnemyObject;

    public SaveManager Saver;

    //Used for troop unlocks and level progression on the map
    public int currentLevel;
    public int maxPlayerTroops;

    //public int newTroopNumber;
    public Unit newTroop;   //Used to set the new troop to the players party when added immediately

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

    //Can be called from a variety of places, and will deal with changing the states that can be used for animations later
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
            //Advances the player, and can deal with the rewards as they are needed
            VictoryScreen.SetActive(true);

            PlayerInfo.LevelKeys[currentLevel] = true;

            if (newTroop != null) {
                PlayerInfo.TroopKeys[newTroop.UnitNumber] = true;
            }

            if (maxPlayerTroops > PlayerInfo.TroopSpaces) {
                PlayerInfo.TroopSpaces = maxPlayerTroops;
            }
            

        }
        else if (losingTeam == 1)
        {
            DefeatScreen.SetActive(true);
            //PlayerPrefs.SetInt("CompletedLevels", 30);
        }
    }

    //Generic level stuff below
    public void StartLevel() {
        StartScreen.SetActive(false);

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
