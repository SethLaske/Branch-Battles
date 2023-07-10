using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Unit initMiner;
    public Unit initFighter;
    public Unit initSpear;
    

    void Start()
    {
        for (int i = -1; i < 11; i++) {
            PlayerInfo.LevelKeys.Add(i, false);
            PlayerInfo.TroopKeys.Add(i, false);
        }
        //PlayerInfo.LevelKeys[-1] = true;
        PlayerInfo.TroopSpaces = 0;

        //PlayerPrefs.SetInt("CompletedLevels", -1);
        //PlayerPrefs.SetInt("UnlockedTroops", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(string LevelSelect) {
        SceneManager.LoadScene(LevelSelect);
        
    }

    public void newGame(string tutorialScene)
    {
        for (int i = -1; i < 11; i++)
        {
            PlayerInfo.LevelKeys.Add(i, false);
            PlayerInfo.TroopKeys.Add(i, false);
        }
        PlayerInfo.TroopSpaces = 0;

        SceneManager.LoadScene(tutorialScene);
    }

    public void loadGame(string LevelScene)
    {
        //Loads previous data

        SceneManager.LoadScene(LevelScene);
    }
}
