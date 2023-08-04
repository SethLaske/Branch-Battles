using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles the generics for each team
public class TeamInfo : MonoBehaviour
{
    public int Team; //set to an int (1 for player, -1 for enemy)
                     //Makes it really easy to control directions and friend/foe

    //Generic info
    [Header("Stats")]
    public int gold;
    public int troopCount;
    public int maxTroopCount;
    //public int gems;
    public int souls;

    public int[] troopCategory = new int[5]; //To be used for the AI

    //A list of all the units that need to be trained
    private List<Unit> spawnUnits = new List<Unit>();
    private float troopTimer = 0;

    //Passive gold quantities
    public int AFKGoldAmount;
    public float AFKGoldTime;
    private float goldCounter = 0;

    //The building troops will spawn from
    public Building barracks;

    [Header ("General's Info")]
    public General general;
    //public float generalSpeed;
    //public float totalSpeed;    //total speed of the troops, which will be used to calculate the Generals Speed
    //public int activeCount;

    public float rallyPoint;    //The point units will move to unless acted upon
    public GameObject rallyFlag;    //Visual representation of where the rally is
    public float spacing = 2f;

    public float advantage = 1; //Buff or Debuff a team

 

    //When Save data is prepared this will replace the individual markings
    //public Unit[] SpawnableUnits = new Unit[5];
    public List<Unit> reinforcements = new List<Unit>();
    public bool reinforced = false;

    public TeamInfo Opponent;

    //Set up a rally near to the first barracks
    void Start()
    {
        
        rallyPoint = barracks.transform.position.x + (Team * 10);
        rallyFlag.transform.position = new Vector3(rallyPoint, rallyFlag.transform.position.y);
        UpdateGeneral();    //Sets the general speed to a predefined value
    }

    // Update is called once per frame
    void Update()
    {
        //Passive gold timer       
        goldCounter += Time.deltaTime;
        if (goldCounter >= AFKGoldTime)
        {
            gold += AFKGoldAmount;
            goldCounter = 0;
        }

        //Runs a loop while troops are in the list
        if (spawnUnits.Count > 0) {
            troopTimer += Time.deltaTime;
            if (troopTimer >= spawnUnits[0].SpawnTime)  //Spawns the next unit queued up 
            {
                Unit FreshMeat = Instantiate(spawnUnits[0], new Vector3(barracks.transform.position.x + (Team * 1f), 0, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
                }

                //Name is set for my use, and team controls the direction
                FreshMeat.name = Team + ": " + FreshMeat.name;
                FreshMeat.General = this;
                FreshMeat.Team = barracks.Team;
               

                //Appies buffs/debuffs
                FreshMeat.HP *= advantage;
                //FreshMeat.MoveSpeed *= advantage;
                FreshMeat.Damage *= advantage;
                //FreshMeat.AttackCooldown /= advantage;

                //The disjoint between adding the troops to the counter and and their speeds to the group is potentially allowing the general to move at sonic speeds
                //totalSpeed += FreshMeat.MoveSpeed;
                //activeCount++;
                UpdateGeneral();

                //Resets timer for next spawn
                spawnUnits.RemoveAt(0);
                troopTimer = 0;
            }

        }
        

    }

    //I just need to switch to this, but the previous set ones might be easier to initialize and edit from the map
    public void SpawnUnit(Unit newUnit) {
        if (gold >= newUnit.Cost && (troopCount + newUnit.TroopSpaces <= maxTroopCount))
        {
            gold -= newUnit.Cost;
            troopCount += newUnit.TroopSpaces;
            troopCategory[newUnit.unitClassification]++;

            spawnUnits.Add(newUnit);

            
        }
    }

    public void SpawnReinforcements() {
        if (reinforced == false) {
            reinforced = true;
            foreach (Unit reinforcement in reinforcements) {
                Unit FreshMeat = Instantiate(reinforcement, new Vector3(barracks.transform.position.x - (Team * Random.Range(2,10)), 0, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
                }

                troopCount += FreshMeat.TroopSpaces;
                troopCategory[FreshMeat.unitClassification]++;

                //Name is set for my use, and team controls the direction
                FreshMeat.name = Team + ": " + FreshMeat.name;
                FreshMeat.General = this;
                FreshMeat.Team = barracks.Team;


                //Appies buffs/debuffs
                FreshMeat.HP *= advantage;
                //FreshMeat.MoveSpeed *= advantage;
                FreshMeat.Damage *= advantage;
                //FreshMeat.AttackCooldown /= advantage;

                //The disjoint between adding the troops to the counter and and their speeds to the group is potentially allowing the general to move at sonic speeds
                //totalSpeed += FreshMeat.MoveSpeed;
                //activeCount++;
                UpdateGeneral();
            }
        
        }
    }


    //Dev Buttons are fun
    public void ForceSpawnUnit(Unit newUnit)
    {
            gold -= newUnit.Cost;
            troopCount += newUnit.TroopSpaces;
            troopCategory[newUnit.unitClassification]++;

            spawnUnits.Add(newUnit);
    }

    public void UseMagic(Magic magic, float distance) {
        if (souls > magic.SoulCost) {
            Instantiate(magic, new Vector3(distance, 0, 0), Quaternion.Euler(new Vector2(0, 0)));
            souls-= magic.SoulCost;
        }
    }

    //Sets the rally point and moves the flag
    public void SetRallyPoint(float rally) {
        if (rallyPoint == rally) {
            return;
        }
        rallyPoint = rally;
        rallyFlag.transform.position = new Vector3(rallyPoint, rallyFlag.transform.position.y);
        //Play flag noise or smth
        rallyFlag.GetComponent<Animator>().SetTrigger("Drop");
    }

    //Immediately goes for the enemy base
    public void Charge() {
        SetRallyPoint(Opponent.barracks.transform.position.x);
    }

    //I think I need to send a command to all units after the battle is over declaring either victory or defeat. Then the units can just die, stop, or play an animation depending on how lazy I am.
    public void Victory() { 
        
    }

    public void Defeat()
    {
        maxTroopCount = 0;
        spawnUnits.Clear();
        Destroy(rallyFlag);
        Destroy(this);
    }

    //Allows the general to be powered up with larger army sizes. Could change it to be only those near to him, but I prefer the inherent aspect to this
    public void UpdateGeneral() {
        if (general == null)
        {
            return;
        }
        //Changing this to be implemented inside the general script, and to be based off of 
        general.CalculateArmyBuffs();
    }
}
