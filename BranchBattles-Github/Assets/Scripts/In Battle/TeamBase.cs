using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Not a great class, but handles game overs for now, and will be fine unless a team can have multiple barracks
public class TeamBase : Building
{
    private Slider healthBar;
    private CameraControls playercamera;
    private LevelManager levelmanager;

    // Finds the stuff itself so I dont need to drag and drop for every base
    void Start()
    {
        Team = General.Team;
        maxHealth = HP;
        playercamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>();
        levelmanager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    public void SetHealthBar(Slider slider) {
        healthBar = slider;
        healthBar.maxValue = HP;
        healthBar.value = HP;
    }
    //Could also call it result screen and condense it to one screen per base
    public override void Die()
    {
        base.Die(); //Destroys the enemy
        General.Defeat();   //Both these classes are currently empty but could be used to send animations to each sides troops
        General.Opponent.Victory();
        levelmanager.GameOver(Team);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (Team == 1) {
            float amount = (.3f * ((maxHealth - HP) / maxHealth)) + .1f;
            playercamera.CallShake(amount, amount);
        }

        if (healthBar != null) {
            healthBar.value = HP;
        }
        
    }


}
