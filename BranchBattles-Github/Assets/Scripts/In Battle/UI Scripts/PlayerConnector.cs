using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Automattically connect the UI prefab to player
public class PlayerConnector : MonoBehaviour
{
    public bool autoInitiate = true;
    public Player player;       //MANUALLY ASSIGN THIS
    private TeamInfo playerTeam;

    [Header("Assign in Prefab")]
    public BattleUI battleUI;
    public LevelManager levelManager;


    //This script is responsible for filling all of the other scripts with their proper values;
    public void InitializeUI()
    {
        playerTeam = player.Peasants;
        player.SetBattleUI(battleUI);
        battleUI.SetPlayer(player);
        playerTeam.general.SetBattleUI(battleUI);
        playerTeam.general.SetLevelManager(levelManager);

           

        playerTeam.barracks.SetHealthBar(battleUI.playerBaseHealth);
        playerTeam.Opponent.barracks.SetHealthBar(battleUI.enemyBaseHealth);

        battleUI.counters.SetTeamInfo(playerTeam);

            

        //This is the good way for when the troops are slowly added and memory works properly. Until then the bad way must be used
        InitTrainButton(battleUI.pacifist1, PlayerInfo.PlayerTroops[0]);
        InitTrainButton(battleUI.soldier1, PlayerInfo.PlayerTroops[1]);
        InitTrainButton(battleUI.soldier2, PlayerInfo.PlayerTroops[2]);
        InitTrainButton(battleUI.soldier3, PlayerInfo.PlayerTroops[3]);
        InitTrainButton(battleUI.soldier4, PlayerInfo.PlayerTroops[4]);


        //soldier1.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier1));
        //soldier2.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier2));
        //soldier3.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier3));
        //soldier4.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier4));

        InitMagicButton(battleUI.magicPrep1, player.magic1);
        battleUI.magicPrep1.onClick.AddListener(player.PrepMagic1);
        InitMagicButton(battleUI.magicPrep2, player.magic2);
        battleUI.magicPrep2.onClick.AddListener(player.PrepMagic2);
        //battleUI.magicPrep1.onClick.AddListener(player.PrepMagic1);

        //battleUI.magicPrep2.onClick.AddListener(player.PrepMagic2);

        battleUI.UIShadow.SetActive(false);
            

        /*TeamBase playerbase = playerTeam.barracks.GetComponent<TeamBase>();
        playerbase.levelmanager = levelManager;

        TeamBase enemybase = playerTeam.barracks.GetComponent<TeamBase>();
        enemybase.levelmanager = levelManager;*/
       
        /*battleUI.playerBase.slider.maxValue = battleUI.playerBase.HealthObject.HP;
        battleUI.enemyBase.slider.maxValue = battleUI.enemyBase.HealthObject.HP;*/
    }

    private void InitTrainButton(Button button, Unit unit) {
        if (unit == null)
        {
            Debug.Log("Was null");
            button.gameObject.SetActive(false);
        }
        else {
            button.onClick.AddListener(() => playerTeam.TrainUnit(unit));
            UnitButtons script = button.GetComponent<UnitButtons>();
            script.SetUnitType(unit);
        }
        
    }

    private void InitMagicButton(Button button, Magic magic) {
        
        button.GetComponent<Image>().sprite = magic.buttonUI;
        MagicButtons script = button.GetComponent<MagicButtons>();
        script.SetMagicType(magic);
    }
}
