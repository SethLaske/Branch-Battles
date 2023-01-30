using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Not a great class, but handles game overs for now, and will be fine unless a team can have multiple barracks
public class TeamBase : Building
{
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Could also call it result screen and condense it to one screen per base
    public override void Die()
    {
        base.Die(); //Destroys the enemy
        General.Defeat();   //Both these classes are currently empty but could be used to send animations to each sides troops
        General.Opponent.Victory();
        UI.SetActive(false);
        if (Team == -1) //Activates the correct UI 
        {
            VictoryScreen.SetActive(true);
        }
        else if (Team == 1)
        {
            DefeatScreen.SetActive(true);
        }
    }

    
}
