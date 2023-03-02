using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float LevelTime = 0.01f; 
    public bool CountDown = false;
    public bool Survival = false;
    public int Direction = 1;

    public TextMeshProUGUI Timer;
    public float Minutes = 0;
    public float Seconds = 0;

    public LevelManager levelmanager;

    void Start()
    {
        if(CountDown || Survival)
        {
            Direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LevelTime += Direction * Time.deltaTime;
        Minutes = Mathf.FloorToInt(LevelTime / 60);
        Seconds = Mathf.FloorToInt(LevelTime % 60);
        Timer.text = string.Format("Timer\n{0:0}:{1:00}", Minutes, Seconds);
        if (LevelTime <= 0) {
            if (CountDown)
            {
                levelmanager.GameOver(1);
            }
            else if (Survival) {
                levelmanager.GameOver(-1);
            }
        }
    }
}
