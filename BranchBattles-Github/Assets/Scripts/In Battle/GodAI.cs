using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodAI : MonoBehaviour
{
    public TeamInfo Peasants;

    public int[] minTroops = new int[5];
    public int[] maxTroops = new int[5];

    void Start()
    {
        //Currently starts by spawning a miner
        Peasants.spawnUnit(Peasants.SpawnableUnits[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Peasants.Barracks != null && Peasants.Opponent.Barracks != null && (Peasants.TroopCount < Peasants.TroopMax)) {
            spawnTroop();

            if (Peasants.TroopCount > 6)
            {
                Peasants.Charge();
            }
            else if (Peasants.TroopCount < 5)
            {
                Peasants.setRallyPoint(15);
            }
        }

        /*

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
        }*/
        
    }

    public void spawnTroop() {
        for (int i = 0; i < minTroops.Length; i++) {
            if (Peasants.troopCategory[i] < minTroops[i]) {
                Peasants.spawnUnit(Peasants.SpawnableUnits[i]);
                return;
            }
        }

        for (int i = 0; i < maxTroops.Length; i++)
        {
            if (Peasants.troopCategory[i] < maxTroops[i])
            {
                Peasants.spawnUnit(Peasants.SpawnableUnits[i]);
                return;
            }
        }

        Peasants.spawnUnit(Peasants.SpawnableUnits[Random.Range(0,5)]);

    }
    public void standardAttack() {
        Peasants.spawnSoldier2();
        Peasants.spawnSoldier1();
        Peasants.spawnSoldier2();
    }
}
