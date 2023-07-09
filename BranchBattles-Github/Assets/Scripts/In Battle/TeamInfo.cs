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
    public int Gems;
    public int Souls;

    public int[] troopCategory = new int[5]; //To be used for the AI

    //A list of all the units that need to be trained
    public List<Unit> SpawnUnits = new List<Unit>();
    public float TroopTimer = 0;

    //Passive gold quantities
    public int AFKGoldAmount;
    public float AFKGoldTime;
    public float GoldCounter = 0;

    //The building troops will spawn from
    public Building Barracks;

    [Header ("General's Info")]
    public General general;
    public float generalSpeed;
    public float TotalSpeed;    //total speed of the troops, which will be used to calculate the Generals Speed
    public int ActiveCount;

    public float RallyPoint;    //The point units will move to unless acted upon
    public GameObject RallyFlag;    //Visual representation of where the rally is
    public float Spacing = 2f;

    public float Advantage = 1; //Buff or Debuff a team

    //All of this can be removed. The player will have their list be maintained in Player info and get pulled by the buttons
    //The enemy can have a list in the AI section
    //Available units
    public Unit Soldier1;
    public Unit Soldier2;
    public Unit Soldier3;
    public Unit Soldier4;

    public Unit Pacifist1;

    //When Save data is prepared this will replace the individual markings
    public Unit[] SpawnableUnits = new Unit[5];
    public List<Unit> Reinforcements = new List<Unit>();
    public bool Reinforced = false;

    public TeamInfo Opponent;

    //Set up a rally near to the first barracks
    void Start()
    {
        
        RallyPoint = Barracks.transform.position.x + (Team * 10);
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
        UpdateGeneral();    //Sets the general speed to a predefined value
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

        //Runs a loop while troops are in the list
        if (SpawnUnits.Count > 0) {
            TroopTimer += Time.deltaTime;
            if (TroopTimer >= SpawnUnits[0].SpawnTime)  //Spawns the next unit queued up 
            {
                Unit FreshMeat = Instantiate(SpawnUnits[0], new Vector3(Barracks.transform.position.x + (Team * 1f), 0, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
                }

                //Name is set for my use, and team controls the direction
                FreshMeat.name = Team + ": " + FreshMeat.name;
                FreshMeat.General = this;
                FreshMeat.Team = Barracks.Team;
               

                //Appies buffs/debuffs
                FreshMeat.HP *= Advantage;
                //FreshMeat.MoveSpeed *= Advantage;
                FreshMeat.Damage *= Advantage;
                //FreshMeat.AttackCooldown /= Advantage;

                //The disjoint between adding the troops to the counter and and their speeds to the group is potentially allowing the general to move at sonic speeds
                TotalSpeed += FreshMeat.MoveSpeed;
                ActiveCount++;
                UpdateGeneral();

                //Resets timer for next spawn
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
            troopCategory[newUnit.unitClassification]++;

            SpawnUnits.Add(newUnit);

            
        }
    }

    public void spawnReinforcements() {
        if (Reinforced == false) {
            Reinforced = true;
            foreach (Unit reinforcement in Reinforcements) {
                Unit FreshMeat = Instantiate(reinforcement, new Vector3(Barracks.transform.position.x - (Team * Random.Range(2,10)), 0, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
                }

                TroopCount += FreshMeat.TroopSpaces;
                troopCategory[FreshMeat.unitClassification]++;

                //Name is set for my use, and team controls the direction
                FreshMeat.name = Team + ": " + FreshMeat.name;
                FreshMeat.General = this;
                FreshMeat.Team = Barracks.Team;


                //Appies buffs/debuffs
                FreshMeat.HP *= Advantage;
                //FreshMeat.MoveSpeed *= Advantage;
                FreshMeat.Damage *= Advantage;
                //FreshMeat.AttackCooldown /= Advantage;

                //The disjoint between adding the troops to the counter and and their speeds to the group is potentially allowing the general to move at sonic speeds
                TotalSpeed += FreshMeat.MoveSpeed;
                ActiveCount++;
                UpdateGeneral();
            }
        
        }
    }


    //Dev Buttons are fun
    public void ForceSpawnUnit(Unit newUnit)
    {
            Gold -= newUnit.Cost;
            TroopCount += newUnit.TroopSpaces;
            troopCategory[newUnit.unitClassification]++;

            SpawnUnits.Add(newUnit);
    }

    public void useMagic(Magic magic, float distance) {
        /*if (Gems > 0) {
            Instantiate(magic, new Vector3(distance, 0, 0), Quaternion.Euler(new Vector2(0, 0)));
            Gems--;
        }*/

        if (Souls > magic.SoulCost) {
            Instantiate(magic, new Vector3(distance, 0, 0), Quaternion.Euler(new Vector2(0, 0)));
            Souls-= magic.SoulCost;
        }
    }

    public void spawnSoldier1()
    {
        if (Gold >= Soldier1.Cost && (TroopCount + Soldier1.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier1.Cost;
            TroopCount += Soldier1.TroopSpaces;
            troopCategory[Soldier1.unitClassification]++;
            SpawnUnits.Add(Soldier1);
        }
    }
    public void spawnSoldier2()
    {
        if (Gold >= Soldier2.Cost && (TroopCount + Soldier2.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier2.Cost;
            TroopCount += Soldier2.TroopSpaces;
            troopCategory[Soldier2.unitClassification]++;
            SpawnUnits.Add(Soldier2);
        }
    }
    public void spawnSoldier3()
    {
        if (Gold >= Soldier3.Cost && (TroopCount + Soldier3.TroopSpaces <= TroopMax))
        {
            Gold -= Soldier3.Cost;
            TroopCount += Soldier3.TroopSpaces;
            troopCategory[Soldier3.unitClassification]++;
            SpawnUnits.Add(Soldier3);
        }
    }
    public void spawnPacifist1()
    {
        if (Gold >= Pacifist1.Cost && (TroopCount + Pacifist1.TroopSpaces <= TroopMax))
        {
            Gold -= Pacifist1.Cost;
            TroopCount += Pacifist1.TroopSpaces;
            troopCategory[Pacifist1.unitClassification]++;
            SpawnUnits.Add(Pacifist1);
        }
    }

    //Sets the rally point and moves the flag
    public void setRallyPoint(float Rally) {
        RallyPoint = Rally;
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
        //Play flag noise or smth
        RallyFlag.GetComponent<Animator>().SetTrigger("Drop");
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

    //Allows the general to be powered up with larger army sizes. Could change it to be only those near to him, but I prefer the inherent aspect to this
    public void UpdateGeneral() {
        if (generalSpeed > 0) {
            if (ActiveCount > 10)
            {
                general.baseSpeed = TotalSpeed / ActiveCount;
            }
            else if (ActiveCount > 0)
            {
                general.baseSpeed = ((TotalSpeed / ActiveCount) - generalSpeed) * ActiveCount/ 10 + generalSpeed;
            }
            else {
                general.baseSpeed = generalSpeed;
            }

            general.Offense.Damage = general.Damage + ActiveCount / 5;
        }
        
    }
}
