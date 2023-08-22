using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePauseMenu : MonoBehaviour
{
    //public LevelManager levelManager;
    public GameObject battleUI;

    public void PauseGame() {
        gameObject.SetActive(true);
        battleUI.SetActive(false);
        LevelManager.gameState = GameState.Paused;
        Time.timeScale = 0;
    }

    public void UnpauseGame() {

        gameObject.SetActive(false);
        battleUI.SetActive(true);
        LevelManager.gameState = GameState.InGame;
        Time.timeScale = 1;
    }

    //Return to level select map

    //Change music

    //Change sound effects


}
