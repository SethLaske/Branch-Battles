using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Automattically connect the UI prefab to player
public class PlayerConnector : MonoBehaviour
{
    public bool autoInitiate = true;
    private Player player;
    private TeamInfo playerTeam;

    public BattleUI battleUI;

    public LevelManager levelManager;


    //Auto
    void Start()
    {
        if (autoInitiate) {

            if (PlayerInfo.PlayerTroops[0] == null)
            {
                levelManager.Saver.LoadPlayer();
                Debug.Log("Player has this many troops " + PlayerInfo.TroopSpaces);
            }

            GameObject PlayerTag = GameObject.FindGameObjectWithTag("Player");  //finds the player
            player = PlayerTag.GetComponent<Player>();
            playerTeam = player.Peasants;
            player.SetBattleUI(battleUI);
            battleUI.SetPlayer(player);
            playerTeam.general.battleUI = battleUI;

            //levelManager.PlayerObject = player.gameObject;
            //levelManager.EnemyObject = playerTeam.Opponent.gameObject;

            battleUI.playerBase.HealthObject = playerTeam.barracks;
            battleUI.playerBase.slider.maxValue = playerTeam.barracks.HP;
            //Debug.Log("Setting up enemy health bar");
            battleUI.enemyBase.HealthObject = playerTeam.Opponent.barracks;
            battleUI.enemyBase.slider.maxValue = playerTeam.Opponent.barracks.HP;

            //battleUI.rally.onClick.AddListener(player.prepRallyPoint);
            //battleUI.charge.onClick.AddListener(playerTeam.Charge);

            battleUI.counters.SetTeamInfo(playerTeam);

            /*
            initButton(pacifist1, playerTeam.Pacifist1);
            initButton(soldier1, playerTeam.Soldier1);
            initButton(soldier2, playerTeam.Soldier2);
            initButton(soldier3, playerTeam.Soldier3);
            initButton(soldier4, playerTeam.Soldier4);
            */

            //This is the good way for when the troops are slowly added and memory works properly. Until then the bad way must be used
            initButton(battleUI.pacifist1, PlayerInfo.PlayerTroops[0]);
            initButton(battleUI.soldier1, PlayerInfo.PlayerTroops[1]);
            initButton(battleUI.soldier2, PlayerInfo.PlayerTroops[2]);
            initButton(battleUI.soldier3, PlayerInfo.PlayerTroops[3]);
            initButton(battleUI.soldier4, PlayerInfo.PlayerTroops[4]);


            //soldier1.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier1));
            //soldier2.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier2));
            //soldier3.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier3));
            //soldier4.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier4));

            battleUI.magicPrep1.onClick.AddListener(player.PrepMagic1);
            battleUI.magicPrep2.onClick.AddListener(player.PrepMagic2);


            

            TeamBase playerbase = playerTeam.barracks.GetComponent<TeamBase>();
            playerbase.levelmanager = levelManager;

            TeamBase enemybase = playerTeam.barracks.GetComponent<TeamBase>();
            enemybase.levelmanager = levelManager;
        }
        else    //Specific for tutorial
        {
            battleUI.playerBase.slider.maxValue = battleUI.playerBase.HealthObject.HP;
            battleUI.enemyBase.slider.maxValue = battleUI.enemyBase.HealthObject.HP;


        }

    }

    public void initButton(Button button, Unit unit) {
        if (unit == null)
        {
            Debug.Log("Was null");
            button.gameObject.SetActive(false);
        }
        else {
            button.onClick.AddListener(() => playerTeam.SpawnUnit(unit));
            UnitButtons script = button.GetComponent<UnitButtons>();
            script.SetUnitType(unit);
        }
        
    }
}
