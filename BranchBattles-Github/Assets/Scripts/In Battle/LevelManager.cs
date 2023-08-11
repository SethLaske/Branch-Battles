using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { 
    Paused,
    InGame,
    Gameover
}

//Level manager ends up in the UI and deals with managing the level
public class LevelManager : MonoBehaviour
{
    public static GameState gameState;
    public bool buttonStart;    //Allows me to choose if I want to press a button to start level

    [Header ("Player Progression Variables")]
    public Unit unlockableTroop;   //Used to set the new troop to the players party when added immediately
    //Used for troop unlocks and level progression on the map
    public int currentLevel;
    public int maxPlayerTroops;

    //public static bool paused = true;   //Could also switch it to semaphore interpretation. Going to hold off for now, but makes sense to have a static variable controlling paused or not
    [Header("Assigned in Prefab")]
    public GameObject StartScreen;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject BattleUI;
    public GameObject BattleUIUnits;

    //public GameObject PlayerObject;
    //public GameObject EnemyObject;

    public SaveManager saver;
    public PlayerConnector playerConnector;
    public AudioSource winSound;


    //public int newTroopNumber;

    

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Paused;
        if (PlayerInfo.PlayerTroops[0] == null)
        {
            saver.LoadPlayer();
            Debug.Log("Player has this many troops " + PlayerInfo.TroopSpaces);
        }

        playerConnector.InitializeUI();

        if (buttonStart)
        {
            //Time.timeScale = 0;
            BattleUI.SetActive(false);

            //PlayerObject.SetActive(false);
            //EnemyObject.SetActive(false);


            StartScreen.SetActive(true);
        }
        else {  //Im assuming for things without a button start the scenes will be appropriately set up already
            StartLevel();
        }
    }

    //Can be called from a variety of places, and will deal with changing the states that can be used for animations later
    public void GameOver(int losingTeam) {
        gameState = GameState.Gameover;

        BattleUI.SetActive(false);
         
        //Assign each unit individually its win or loss state
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
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
            winSound.Play();

            PlayerInfo.LevelKeys[currentLevel] = true;

            if (unlockableTroop != null) {
                PlayerInfo.TroopKeys[unlockableTroop.UnitNumber] = true;
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
        LevelManager.gameState = GameState.InGame;

        StartScreen.SetActive(false);

        //PlayerObject.SetActive(true);
        //EnemyObject.SetActive(true);
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
