using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    private float LevelTime = 0.01f; 
    public bool countDown = false;
    public bool survival = false;
    private int direction = 1;

    public TextMeshProUGUI Timer;
    private float minutes = 0;
    private float seconds = 0;

    public LevelManager levelmanager;

    private bool paused = false;

    void Start()
    {
        if(countDown || survival)
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == true) {
            return;
        }

        LevelTime += direction * Time.deltaTime;
        minutes = Mathf.FloorToInt(LevelTime / 60);
        seconds = Mathf.FloorToInt(LevelTime % 60);
        Timer.text = string.Format("Timer\n{0:0}:{1:00}", minutes, seconds);
        if (LevelTime <= 0) {
            if (countDown)
            {
                levelmanager.GameOver(1);
            }
            else if (survival) {
                levelmanager.GameOver(-1);
            }
        }
    }
}
