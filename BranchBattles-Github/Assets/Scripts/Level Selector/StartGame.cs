using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("CompletedLevels", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(string LevelSelect) {
        SceneManager.LoadScene(LevelSelect);
    }
}
