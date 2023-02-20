using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodAI : MonoBehaviour
{
    public TeamInfo Peasants;  

    
    void Start()
    {
        //Currently starts by spawning a miner
        Peasants.spawnPacifist1();
    }

    // Update is called once per frame
    void Update()
    {
        if (Peasants.Barracks != null && Peasants.Opponent.Barracks != null) {  //Always running until the game is over
            
            //Always tries to train
            if (Peasants.TroopCount < 4)
            {
                Peasants.spawnSoldier1();
            }
            else if (Peasants.TroopCount == 7)
            {
                Peasants.spawnSoldier3();
            }
            else
            {
                standardAttack();   //Just a generic attack pattern
            }

            //If there are enough troops, charge, and if multiple die and arent being replaced, retreat
            if (Peasants.TroopCount > 6)
            {
                Peasants.Charge();
            }
            else if (Peasants.TroopCount < 5)
            {
                Peasants.setRallyPoint(15);
            }
        }
        
    }

    public void standardAttack() {
        Peasants.spawnSoldier2();
        Peasants.spawnSoldier1();
        Peasants.spawnSoldier2();
    }
}
