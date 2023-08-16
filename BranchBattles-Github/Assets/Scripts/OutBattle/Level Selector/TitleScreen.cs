using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Button continueGame;

    public Unit initMiner;
    public Unit initFighter;
    public Unit initSpear;

    public SaveManager saveManager;

    public bool forceNewGame;
    void Start()
    {
        //load game
        saveManager.LoadPlayer();
        
        if (PlayerInfo.PlayerTroops[0] == null || forceNewGame)
        {
            continueGame.interactable = false;
        }
        else {
            continueGame.interactable = true;
            Debug.Log(PlayerInfo.PlayerTroops[0].unitName);
        }
    }



    /*public void startGame(string LevelSelect) {
        SceneManager.LoadScene(LevelSelect);
        
    }*/

    //Technically this doesn't save to new game so I will need a save button later
    public void NewGame(string tutorialScene)
    {
        PlayerInfo.ClearPlayerInfo();

        for (int i = -1; i < 11; i++)
        {
            PlayerInfo.LevelKeys.Add(i, false);
            PlayerInfo.TroopKeys.Add(i, false);
        }
        PlayerInfo.LevelKeys[0] = true;
        PlayerInfo.TroopSpaces = 2;

        SceneManager.LoadScene(tutorialScene);
    }

    public void ContinueGame(string LevelScene)
    {
        //Loads previous data

        SceneManager.LoadScene(LevelScene);
    }

    public void NewPlayer() {
        saveManager.ClearPlayerSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
