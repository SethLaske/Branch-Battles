using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Automattically connect the UI prefab to player
public class PlayerConnector : MonoBehaviour
{
    public bool autoInitiate = true;
    public Player player;
    public TeamInfo playerTeam;

    /*public BaseHealthBar playerBase;    //set in prefab
    public BaseHealthBar enemyBase;     //set in prefab

    public Button Rally;
    public Button Charge;

    public GoldCounter counters;

    public Button pacifist1;
    //public Button pacifist2;

    public Button soldier1;
    public Button soldier2;
    public Button soldier3;
    public Button soldier4;

    public Button magicPrep1;
    public Button magicPrep2;*/

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
            player.battleUI = battleUI;
            battleUI.player = player;
            playerTeam.general.battleUI = battleUI;

            //levelManager.PlayerObject = player.gameObject;
            //levelManager.EnemyObject = playerTeam.Opponent.gameObject;

            battleUI.playerBase.HealthObject = playerTeam.Barracks;
            battleUI.playerBase.slider.maxValue = playerTeam.Barracks.HP;
            //Debug.Log("Setting up enemy health bar");
            battleUI.enemyBase.HealthObject = playerTeam.Opponent.Barracks;
            battleUI.enemyBase.slider.maxValue = playerTeam.Opponent.Barracks.HP;

            //battleUI.rally.onClick.AddListener(player.prepRallyPoint);
            //battleUI.charge.onClick.AddListener(playerTeam.Charge);

            battleUI.counters.player = playerTeam;

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

            battleUI.magicPrep1.onClick.AddListener(player.prepMagic1);
            battleUI.magicPrep2.onClick.AddListener(player.prepMagic2);


            

            TeamBase playerbase = playerTeam.Barracks.GetComponent<TeamBase>();
            playerbase.levelmanager = levelManager;

            TeamBase enemybase = playerTeam.Barracks.GetComponent<TeamBase>();
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
            button.onClick.AddListener(() => playerTeam.spawnUnit(unit));
            UnitButtons script = button.GetComponent<UnitButtons>();
            script.SetUnitType(unit);
        }
        
    }
}
