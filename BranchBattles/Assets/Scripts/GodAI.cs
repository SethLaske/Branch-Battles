using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodAI : MonoBehaviour
{
    public TeamInfo Peasants;

    //public TeamInfo TheBoys;
    // Start is called before the first frame update
    void Start()
    {
        Peasants.spawnPacifist1();
        Peasants.spawnPacifist1();
    }

    // Update is called once per frame
    void Update()
    {
        if (Peasants.TroopCount < 4)
        {
            Peasants.spawnSoldier1();
        }
        else if (Peasants.TroopCount == 7)
        {
            Peasants.spawnSoldier3();
        }
        else {
            standardAttack();
        }
        
        //Peasants.spawnPacifist1();
        if (Peasants.TroopCount > 6) {
            Peasants.Charge();
        }
        else if (Peasants.TroopCount < 5)
        {
            Peasants.setRallyPoint(15);
        }
    }

    public void standardAttack() {
        Peasants.spawnSoldier1();
        Peasants.spawnSoldier2();
        Peasants.spawnSoldier2();
    }
}
