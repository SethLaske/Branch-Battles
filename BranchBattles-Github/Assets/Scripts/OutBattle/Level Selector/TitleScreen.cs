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
    void Start()
    {
        //load game
        saveManager.LoadPlayer();
        if (PlayerInfo.PlayerTroops[0] == null)
        {
            continueGame.interactable = false;
        }
        else {
            continueGame.interactable = true;
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
        //PlayerInfo.LevelKeys[-1] = true;
        PlayerInfo.TroopSpaces = 0;

        SceneManager.LoadScene(tutorialScene);
    }

    public void ContinueGame(string LevelScene)
    {
        //Loads previous data

        SceneManager.LoadScene(LevelScene);
    }
}
