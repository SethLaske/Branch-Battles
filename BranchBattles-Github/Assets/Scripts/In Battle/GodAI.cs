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
            Peasants.spawnUnit(Starter);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Peasants.Barracks != null && Peasants.Opponent.Barracks != null && (Peasants.TroopCount < Peasants.TroopMax)) {
            spawnTroop();
            positionTroops();
        }
        if (Peasants.Barracks.HP < 75) {
            Peasants.spawnReinforcements();
        }
    }

    public void positionTroops() {
        int MySoldiers = Peasants.TroopCount - Peasants.troopCategory[0];
        int EnemySoldiers = Peasants.Opponent.TroopCount - Peasants.Opponent.troopCategory[0];

        if (MySoldiers - EnemySoldiers > attackDifferential) {
            Peasants.Charge();
        } else if (EnemySoldiers > MySoldiers) {
            Peasants.setRallyPoint(10);
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
                Peasants.spawnUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                return;
            }
        }


        if (Peasants.Gold < maxGoldThreshold) {
            return;
        }

        for (int i = 0; i < maxTroops.Length; i++)
        {
            if (Peasants.troopCategory[i] < maxTroops[i])
            {
                Peasants.spawnUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                //Debug.Log("adding to max");
                return;
            }
        }
        
        int RandCat = Random.Range(1, SpawnableUnits.Length);
        Peasants.spawnUnit(SpawnableUnits[RandCat].PotentialSpawns[Random.Range(0, SpawnableUnits[RandCat].PotentialSpawns.Count)]);

    }

    

}
