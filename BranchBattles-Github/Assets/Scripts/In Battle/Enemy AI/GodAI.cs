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

    public Magic lightningMagic;


    void Start()
    {
        foreach (Unit Starter in StartSequence) {
            controlledTeam.TrainUnit(Starter);
        }

        controlledTeam.SetRallyPoint(10);

        if (lightningMagic != null)
        {
            lightningMagic = Instantiate(lightningMagic, Vector3.zero, Quaternion.identity);
            lightningMagic.SetTeamInfo(controlledTeam);
            lightningMagic.gameObject.SetActive(false);
        }
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
        UseLightningMagic();
    }

    private void PositionArmy() {

        //If either side is at the full troop count charge. Puts the AI at a disadvantage, but I dont want games dragging out for no reason. Might change that for different game modes
        if (controlledTeam.troopCount == controlledTeam.maxTroopCount || controlledTeam.Opponent.troopCount == controlledTeam.Opponent.maxTroopCount || controlledTeam.maxTroopCount == 0) {
            controlledTeam.Charge();
        }

        //Currently defends until the AI has a sufficiently high army count and then attacks. Retreats once it is slightly outnumbered
        int MySoldiers = controlledTeam.troopCount - controlledTeam.troopCategory[0];
        int EnemySoldiers = controlledTeam.Opponent.troopCount - controlledTeam.Opponent.troopCategory[0];

        if (MySoldiers - EnemySoldiers > soldierDifferenceToAttack) {
            controlledTeam.Charge();
        } else if (EnemySoldiers - 2 > MySoldiers) {
            controlledTeam.SetRallyPoint(10);
        }


    }

    private void spawnTroop() {
        if (controlledTeam.troopCount >= controlledTeam.maxTroopCount) return;


        //Fills out the count for all of the minimum requirements starting at the miners and working up
        for (int i = 0; i < minTroops.Length; i++) {
            if (controlledTeam.troopCategory[i] < minTroops[i] && SpawnableUnits.Length >= i) {
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

    private void UseLightningMagic() {
        if (lightningMagic == null) return;

        if (lightningMagic.soulCost > controlledTeam.souls) return;

        //Hitting the enemy with anything to try and survive
        if (controlledTeam.troopCount < 4 && controlledTeam.Opponent.troopCount > 4) {
            //Find the front most enemy and spawn lightning there
            Vector3 hitPosition = FindClosestEnemy();
            lightningMagic.SendMagicToLocation(controlledTeam.barracks.transform.position.x, hitPosition.x);
        }

        //Just trying to hit as many enemies as possible
        if (controlledTeam.Opponent.troopCount > 10) { 
            
            Vector3 hitPosition = FindLargestEnemyGroup();
            lightningMagic.SendMagicToLocation(controlledTeam.barracks.transform.position.x, hitPosition.x - 3.5f);
        }

        //Hitting the enemy with lightning to try and kill the miners
        if (controlledTeam.Opponent.troopCount < 10 && controlledTeam.Opponent.troopCategory[0] >= 3) {
            //Send lightning to behind the rally flag
            lightningMagic.SendMagicToLocation(controlledTeam.barracks.transform.position.x, controlledTeam.Opponent.rallyPoint - 4.5f);
        }


    }

    private Vector3 FindClosestEnemy() {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        if (allUnits.Length <= 0)
        {
            return Vector3.zero;
        }

        Unit closestUnit = allUnits[0];

        foreach (Unit unit in allUnits)
        {
            if (unit.transform.position.x > closestUnit.transform.position.x)
            {
                closestUnit = unit;
            }
        }

        return closestUnit.transform.position;
    }

    private Vector3 FindLargestEnemyGroup() {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        if (allUnits.Length <= 0)
        {
            return Vector3.zero;
        }


        Unit largestGroupUnit = allUnits[0];
        int largestGroupSize = 0;

        foreach (Unit unit in allUnits)
        {
            int groupSize = CountUnitsNearby(unit.transform.position);
            if (groupSize > largestGroupSize)
            {
                largestGroupSize = groupSize;
                largestGroupUnit = unit;
            }
        }

        //Debug.Log("Largest group size: " + largestGroupSize);
        //Debug.Log("Largest group unit: " + largestGroupUnit.name);

        return largestGroupUnit.transform.position;
    }

    private int CountUnitsNearby(Vector3 center) {
        int count = 0;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, 5);
        foreach (Collider2D collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null && unit.Team != controlledTeam.Team)
            {
                count++;
            }

        }

        return count;
    }
}
