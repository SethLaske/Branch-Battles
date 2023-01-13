using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamInfo : MonoBehaviour
{
    public int Team; //probably flipped to a int, but can be used to compare when clicking on things

    public int Gold;
    public int TroopCount;
    public int TroopMax;

    public List<Unit> SpawnUnits = new List<Unit>();
    public float TroopTimer = 0;

    public int AFKGoldAmount;
    public float AFKGoldTime;
    public float GoldCounter = 0;

    public Building Barracks;

    //public string Mode; //Indicator for rally or charge
    public float SavedRallyPoint;   //Assuming switching to Attack functions the same as setting the rally point behind the enemy base, we can just save the rally point to recover it? Or just force the player to set it each time
    public bool PassRally;
    public float RallyPoint;    //Allow Player to set a rally point
    public GameObject RallyFlag;

    public float Advantage = 1; //Buff or Debuff a team

    //Available units
    public Unit Soldier1;
    public Unit Soldier2;
    public Unit Soldier3;

    public Unit Pacifist1;

    public TeamInfo Opponent;

    // Start is called before the first frame update
    void Start()
    {
        //Mode = "Rally";
        RallyPoint = Barracks.transform.position.x + (Team * 12);
    }

    // Update is called once per frame
    void Update()
    {
        GoldCounter += Time.deltaTime;
        if (GoldCounter >= AFKGoldTime)
        {
            Gold += AFKGoldAmount;
            GoldCounter = 0;
        }

        if (SpawnUnits.Count > 0) {
            TroopTimer += Time.deltaTime;
            if (TroopTimer >= SpawnUnits[0].SpawnTime)
            {
                Unit FreshMeat = Instantiate(SpawnUnits[0], new Vector3(Barracks.transform.position.x + (Team * 3), -2.25f, 0), Quaternion.identity);
                if (Team < 0)
                {
                    FreshMeat.transform.Rotate(new Vector3(0, 180, 0));
                }
                FreshMeat.General = this;
                FreshMeat.Team = Barracks.Team;
                FreshMeat.MoveSpeed *= Team;

                FreshMeat.HP *= Advantage;
                FreshMeat.Damage *= Advantage;
                FreshMeat.MoveSpeed *= Advantage;

                SpawnUnits.RemoveAt(0);
                TroopTimer = 0;
            }

        }

        if (PassRally == true && Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            setRallyPoint(Worldpos.x);
            PassRally = false;
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

    public void prepRallyPoint()
    {
        PassRally = true;
    }



    public void setRallyPoint(float Rally) {
        RallyPoint = Rally;
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
    }

    public void Charge() {
        RallyPoint = Opponent.Barracks.transform.position.x;
        RallyFlag.transform.position = new Vector3(RallyPoint, RallyFlag.transform.position.y);
    }

    //I think I need to send a command to all units after the battle is over declaring either victory or defeat. Then the units can just die, stop, or play an animation depending on how lazy I am.
    public void Victory() { 
        
    }

    public void Defeat()
    {
        Destroy(RallyFlag);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
