using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CategorySpawns
{
    //public int Category;
    public List<Unit> PotentialSpawns = new List<Unit>();
}



public class GodAI : MonoBehaviour
{
    public TeamInfo controlledTeam;
    public List<Unit> StartSequence = new List<Unit>();
    public int midGoldThreshold = 75;
    public int soldierDifferenceToAttack;
    public int[] minTroops = new int[5];
    public int[] midTroops = new int[5];
    public CategorySpawns[] SpawnableUnits;
    public CategorySpawns finalSpawns;

    

    void Start()
    {
        foreach (Unit Starter in StartSequence) {
            controlledTeam.TrainUnit(Starter);
        }

        controlledTeam.SetRallyPoint(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame)
        {
            return;
        }

        if (controlledTeam.barracks == null || controlledTeam.Opponent.barracks == null) {
            return;
        }



        spawnTroop();
        PositionArmy();
    }

    private void PositionArmy() {

        //If either side is at the full troop count charge. Puts the AI at a disadvantage, but I dont want games dragging out for no reason. Might change that for different game modes
        if (controlledTeam.troopCount == controlledTeam.maxTroopCount || controlledTeam.Opponent.troopCount == controlledTeam.Opponent.maxTroopCount) {
            controlledTeam.Charge();
        }

        //Currently defends until the AI has a sufficiently high army count and then attacks. Retreats once it is slightly outnumbered
        int MySoldiers = controlledTeam.troopCount - controlledTeam.troopCategory[0];
        int EnemySoldiers = controlledTeam.Opponent.troopCount - controlledTeam.Opponent.troopCategory[0];

        if (MySoldiers - EnemySoldiers > soldierDifferenceToAttack) {
            controlledTeam.Charge();
        } else if (EnemySoldiers-2 > MySoldiers) {
            controlledTeam.SetRallyPoint(10);
        }

       
    }

    private void spawnTroop() {
        if (controlledTeam.troopCount >= controlledTeam.maxTroopCount) return;


        //Fills out the count for all of the minimum requirements starting at the miners and working up
        for (int i = 0; i < minTroops.Length; i++) {
            if (controlledTeam.troopCategory[i] < minTroops[i]   && SpawnableUnits.Length >= i) {
                controlledTeam.TrainUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                return;
            }
        }

        //Holds off on training until it has more gold (should be the cost of the highest priced unit)
        if (controlledTeam.gold < midGoldThreshold) {
            return;
        }


        for (int i = 0; i < midTroops.Length; i++)
        {
            if (controlledTeam.troopCategory[i] < midTroops[i] && SpawnableUnits.Length >= i)
            {
                controlledTeam.TrainUnit(SpawnableUnits[i].PotentialSpawns[Random.Range(0, SpawnableUnits[i].PotentialSpawns.Count)]);
                //Debug.Log("adding to mid");
                return;
            }
        }
        
        //Using this to control the late game spawns
        controlledTeam.TrainUnit(finalSpawns.PotentialSpawns[Random.Range(0, finalSpawns.PotentialSpawns.Count)]);

    }

    

}
