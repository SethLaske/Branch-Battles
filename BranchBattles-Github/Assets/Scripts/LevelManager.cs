using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject UI;

    public int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        
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

    
}
