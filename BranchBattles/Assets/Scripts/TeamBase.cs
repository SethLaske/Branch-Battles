using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        base.Die();
        General.Defeat();
        UI.SetActive(false);
        if (Team == -1)
        {
            VictoryScreen.SetActive(true);
        }
        else if (Team == 1)
        {
            DefeatScreen.SetActive(true);
        }
    }

    
}
