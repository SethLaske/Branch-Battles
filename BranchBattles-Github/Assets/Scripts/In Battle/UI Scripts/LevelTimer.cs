using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float levelTime = 0.01f; 
    public bool countDown = false;
    public bool survival = false;
    private int direction = 1;

    public TextMeshProUGUI Timer;
    private float minutes = 0;
    private float seconds = 0;

    public LevelManager levelmanager;

    void Start()
    {
        if (countDown || survival)
        {
            direction = -1;

        }
        else {
            levelTime = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame)
        {
            return;
        }

        levelTime += direction * Time.deltaTime;
        minutes = Mathf.FloorToInt(levelTime / 60);
        seconds = Mathf.FloorToInt(levelTime % 60);
        Timer.text = string.Format("Timer\n{0:0}:{1:00}", minutes, seconds);
        if (levelTime <= 0) {
            if (countDown)
            {
                levelmanager.GameOver(1);
                //Destroy(this.gameObject);
            }
            else if (survival) {
                levelmanager.GameOver(-1);
                //Destroy(this.gameObject);
            }
        }
    }
}
