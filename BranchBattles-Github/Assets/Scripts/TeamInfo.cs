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
    public int Gold;
    public int TroopCount;
    public int TroopMax;

    //A list of all the units that need to be trained
    public List<Unit> SpawnUnits = new List<Unit>();
    public float TroopTimer = 0;

    //Passive gold quantities
    public int AFKGoldAmount;
    public float AFKGoldTime;
    public float GoldCounter = 0;

    //The building troops will spawn from
    public Building Barracks;

    
    public float RallyPoint;    //The point units will move to unless acted upon
    public GameObject RallyFlag;    //Visual representation of where the rally is

    public float Advantage = 1; //Buff or Debuff a team

    //Available units
    public Unit Soldier1;
    public Unit Soldier2;
    public Unit Soldier3;

    public Unit Pacifist1;

    public TeamInfo Opponent;

    //Set up a rally near to the first barracks
    void Start()
    {
        
        RallyPoint = Barracks.transform.position.x + (Team * 14);
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Passive gold timer
        GoldCounter += Time.deltaTime;
        if (GoldCounter >= AFKGoldTime)
        {
            Gold += AFKGoldAmount;
            GoldCounter = 0;
        }

        if (SpawnUnits.Count > 0) {
            TroopTimer += Time.deltaTime;
            if (TroopTimer >= SpawnUnits[0].SpawnTime)  //Spawns the next unit queued up 
            {
                Unit FreshMeat = Instantiate(SpawnUnits[0], new Vector3(Barracks.transform.position.x + (Team * 3), -2.25f, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0));
                }

                //Name is set for my use, and team controls the direction
                FreshMeat.name = Team + ": " + FreshMeat.name;
                FreshMeat.General = this;
                FreshMeat.Team = Barracks.Team;
                FreshMeat.MoveSpeed *= Team;

                //Appies buffs/debuffs
                FreshMeat.HP *= Advantage;
                FreshMeat.MoveSpeed *= Advantage;
                FreshMeat.Damage *= Advantage;
                FreshMeat.AttackCooldown /= Advantage;

                SpawnUnits.RemoveAt(0);
                TroopTimer = 0;
            }

        }
        

    }

    //I just need to switch to this, but the previous set ones might be easier to initialize and edit from the map
    public void spawnUnit(Unit newUnit) {
        if (Gold >= newUnit.Cost && (TroopCount + newUnit.TroopSpaces <= TroopMax))
        {
            Gold -= newUnit.Cost;
            TroopCount += newUnit.TroopSpaces;

            SpawnUnits.Add(newUnit);

            
        }
    }

    public void spawnSoldier1()
    {
        if (Gold >= Soldier1.Cost && (TroopCount + Soldier1.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier1.Cost;
            TroopCount += Soldier1.TroopSpaces;

            SpawnUnits.Add(Soldier1);

            //Unit newEnemy = Instantiate(Unit1, new Vector3(Barracks.transform.position.x, -2.25f, 0), Quaternion.identity);

            //newEnemy.Team = Barracks.Team;
            //Could turn Team into +/- to also set walk direction for the enemy
        }


    }

    public void spawnSoldier2()
    {
        if (Gold >= Soldier2.Cost && (TroopCount + Soldier2.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier2.Cost;
            TroopCount += Soldier2.TroopSpaces;

            SpawnUnits.Add(Soldier2);

            //Unit newEnemy = Instantiate(Unit1, new Vector3(Barracks.transform.position.x, -2.25f, 0), Quaternion.identity);

            //newEnemy.Team = Barracks.Team;
            //Could turn Team into +/- to also set walk direction for the enemy
        }


    }

    public void spawnSoldier3()
    {
        if (Gold >= Soldier3.Cost && (TroopCount + Soldier3.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier3.Cost;
            TroopCount += Soldier3.TroopSpaces;

            SpawnUnits.Add(Soldier3);

            //Unit newEnemy = Instantiate(Unit1, new Vector3(Barracks.transform.position.x, -2.25f, 0), Quaternion.identity);

            //newEnemy.Team = Barracks.Team;
            //Could turn Team into +/- to also set walk direction for the enemy
        }


    }

    public void spawnPacifist1()
    {
        if (Gold >= Pacifist1.Cost && (TroopCount + Pacifist1.TroopSpaces <= TroopMax))
        {
            Gold -= Pacifist1.Cost;
            TroopCount += Pacifist1.TroopSpaces;

            SpawnUnits.Add(Pacifist1);
        }


    }

    //Sets the rally point and moves the flag
    public void setRallyPoint(float Rally) {
        RallyPoint = Rally;
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
    }

    //Immediately goes for the enemy base
    public void Charge() {
        setRallyPoint(Opponent.Barracks.transform.position.x);
    }

    //I think I need to send a command to all units after the battle is over declaring either victory or defeat. Then the units can just die, stop, or play an animation depending on how lazy I am.
    public void Victory() { 
        
    }

    public void Defeat()
    {
        Destroy(RallyFlag);
        Destroy(this);
    }

    
}
