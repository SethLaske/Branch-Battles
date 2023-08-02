using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CategorySpawns
{
    public int Category;
    public List<Unit> PotentialSpawns = new List<Unit>();
}



public class GodAI : MonoBehaviour
{
    public TeamInfo Peasants;
    public List<Unit> StartSequence = new List<Unit>();
    public int maxGoldThreshold = 75;
    public int attackDifferential;
    public int[] minTroops = new int[5];
    public int[] maxTroops = new int[5];
    public CategorySpawns[] SpawnableUnits;

    

    void Start()
    {
        foreach (Unit Starter in StartSequence) {
            Peasants.SpawnUnit(Starter);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Peasants.barracks != null && Peasants.Opponent.barracks != null && (Peasants.troopCount < Peasants.maxTroopCount)) {
            spawnTroop();
            positionTroops();
        }
        if (Peasants.barracks.HP < 75) {
            Peasants.SpawnReinforcements();
        }
    }

    public void positionTroops() {
        int MySoldiers = Peasants.troopCount - Peasants.troopCategory[0];
        int EnemySoldiers = Peasants.Opponent.troopCount - Peasants.Opponent.troopCategory[0];

        if (MySoldiers - EnemySoldiers > attackDifferential) {
            Peasants.Charge();
        } else if (EnemySoldiers > MySoldiers) {
            Peasants.SetRallyPoint(10);
        }

        /*
        if (Peasants.ActiveCount > 12) {
            Peasants.Charge();
        }
        else if (Peasants.ActiveCount < 7){
            Peasants.setRallyPoint(10);
        }*/
        
    
    }

    public void spawnTroop() {

        
        for (int i = 0; i < minTroops.Length; i++) {
            if (Peasants.troopCategory[i] < minTroops[i]) {
                //Debug.Log("adding to min");
                Peasants.SpawnUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                return;
            }
        }


        if (Peasants.gold < maxGoldThreshold) {
            return;
        }

        for (int i = 0; i < maxTroops.Length; i++)
        {
            if (Peasants.troopCategory[i] < maxTroops[i])
            {
                Peasants.SpawnUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                //Debug.Log("adding to max");
                return;
            }
        }
        
        int RandCat = Random.Range(1, SpawnableUnits.Length);
        Peasants.SpawnUnit(SpawnableUnits[RandCat].PotentialSpawns[Random.Range(0, SpawnableUnits[RandCat].PotentialSpawns.Count)]);

    }

    

}
