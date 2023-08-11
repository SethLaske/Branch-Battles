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
    //Passive gold quantities
    public int AFKGoldAmount;
    public float AFKGoldTime;
    private float goldCounter = 0;

    public int gold;
    public int troopCount;
    public int maxTroopCount;
    //public int gems;
    public int souls;
    
    public float advantage = 1; //Buff or Debuff a team

    public int[] troopCategory = new int[5]; //To be used for the AI

    //A list of all the units that need to be trained
    private List<Unit> spawnUnits = new List<Unit>();
    private float troopTimer = 0;

    public General general;

    //The building troops will spawn from
    public TeamBase barracks;
    public float rallyPoint;    //The point units will move to unless acted upon
    public GameObject rallyFlag;    //Visual representation of where the rally is
    public float spacing = 2f;


    public TeamInfo Opponent;

    public AudioSource warHorn;

    //Set up a rally near to the first barracks
    void Start()
    {
        SetRallyPoint(barracks.transform.position.x + (Team * 10));
        //rallyPoint = barracks.transform.position.x + (Team * 10);
        //rallyFlag.transform.position = new Vector3(rallyPoint, rallyFlag.transform.position.y);
        UpdateGeneral();   
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame)
        {
            return;
        }

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
                SpawnUnit(spawnUnits[0]);
                spawnUnits.RemoveAt(0);
                troopTimer = 0;
            }

        }
        

    }

    //Typically called by this script, but also allows for ways of reinforcing or surprise spawns
    //Setting the default to spawn in the normal position
    public void SpawnUnit(Unit newUnit, Vector3 position = default)
    {
        if (position == default) {
            position = new Vector3(barracks.transform.position.x + (Team * 1f), 0, 0);
        }

        Unit freshMeat = Instantiate(newUnit, position, Quaternion.identity);

        if (Team < 0)
        {
            freshMeat.transform.localScale = new Vector3(-1, 1,1);
            //freshMeat.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
        }

        //Name is set for my use, and team controls the direction
        freshMeat.name = Team + ": " + freshMeat.name;
        freshMeat.General = this;
        freshMeat.Team = Team;

        //Appies buffs/debuffs
        freshMeat.HP *= advantage;
        freshMeat.Damage *= advantage;
        
        UpdateGeneral();
    }


    //Called by the buttons and AI
    public void TrainUnit(Unit newUnit) {
        if (gold >= newUnit.Cost && (troopCount + newUnit.TroopSpaces <= maxTroopCount))
        {
            gold -= newUnit.Cost;
            troopCount += newUnit.TroopSpaces;
            troopCategory[newUnit.unitClassification]++;

            spawnUnits.Add(newUnit);
        }
    }


    /*public void UseMagic(Magic magic, float distance) {
        if (souls > magic.SoulCost) {
            Instantiate(magic, new Vector3(distance, 0, 0), Quaternion.Euler(new Vector2(0, 0)));
            souls-= magic.SoulCost;
        }
    }*/


    //Sets the rally point and moves the flag
    public void SetRallyPoint(float rally) {
        if (rallyPoint == rally) {
            return;
        }
        rallyPoint = rally;
        rallyFlag.transform.position = new Vector3(rallyPoint, rallyFlag.transform.position.y);
        //Play flag noise or smth
        rallyFlag.GetComponent<Animator>().SetTrigger("Drop");

        //Debug.Log("Team: " + Team + " is Setting Rally at: " + rally);
    }

    public void ReloadRallyFlag() {
        rallyFlag.transform.position = new Vector3(rallyPoint, rallyFlag.transform.position.y);
        //Play flag noise or smth
        //rallyFlag.GetComponent<Animator>().SetTrigger("Drop");
    }

    //Immediately goes for the enemy base
    public void Charge() {
        if (rallyPoint == Opponent.barracks.transform.position.x)
        {
            return;
        }
        SetRallyPoint(Opponent.barracks.transform.position.x);
        
        warHorn.Play();
    }

    //I think I need to send a command to all units after the battle is over declaring either victory or defeat. Then the units can just die, stop, or play an animation depending on how lazy I am.
    public void Victory() {
        maxTroopCount = 0;
        spawnUnits.Clear();
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
        Debug.Log("Calculating general buffs");
    }
}
