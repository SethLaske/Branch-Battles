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

    public BaseHealthBar playerBase;    //set in prefab
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
    public Button magicPrep2;




    //Auto
    void Start()
    {
        if (autoInitiate) {
            GameObject PlayerTag = GameObject.FindGameObjectWithTag("Player");  //finds the player
            player = PlayerTag.GetComponent<Player>();
            playerTeam = player.Peasants;

            playerBase.HealthObject = playerTeam.Barracks;
            playerBase.slider.maxValue = playerTeam.Barracks.HP;
            Debug.Log("Setting up enemy health bar");
            enemyBase.HealthObject = playerTeam.Opponent.Barracks;
            enemyBase.slider.maxValue = playerTeam.Opponent.Barracks.HP;

            Rally.onClick.AddListener(player.prepRallyPoint);
            Charge.onClick.AddListener(playerTeam.Charge);

            counters.player = playerTeam;

            initButton(pacifist1, playerTeam.Pacifist1);
            //pacifist1.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Pacifist1));

            initButton(soldier1, playerTeam.Soldier1);
            initButton(soldier2, playerTeam.Soldier2);
            initButton(soldier3, playerTeam.Soldier3);
            initButton(soldier4, playerTeam.Soldier4);
            
            //soldier1.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier1));
            //soldier2.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier2));
            //soldier3.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier3));
            //soldier4.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier4));

            magicPrep1.onClick.AddListener(player.prepMagic1);
            magicPrep2.onClick.AddListener(player.prepMagic2);
        }
        else    //Specific for tutorial
        {
            playerBase.slider.maxValue = playerBase.HealthObject.HP;
            enemyBase.slider.maxValue = enemyBase.HealthObject.HP;


        }

    }

    public void initButton(Button button, Unit unit) {
        button.onClick.AddListener(() => playerTeam.spawnUnit(unit));
        UnitButtons script = button.GetComponent<UnitButtons>();
        script.setUnitType(unit);
    }
}
