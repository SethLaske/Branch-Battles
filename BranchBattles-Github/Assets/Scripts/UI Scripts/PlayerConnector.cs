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




    // Start is called before the first frame update
    void Start()
    {
        if (autoInitiate) {
            GameObject PlayerTag = GameObject.FindGameObjectWithTag("Player");
            player = PlayerTag.GetComponent<Player>();
            playerTeam = player.Peasants;

            playerBase.HealthObject = playerTeam.Barracks;
            enemyBase.HealthObject = playerTeam.Opponent.Barracks;

            Rally.onClick.AddListener(player.prepRallyPoint);
            Charge.onClick.AddListener(playerTeam.Charge);

            counters.player = playerTeam;

            pacifist1.onClick.AddListener(playerTeam.spawnPacifist1);

            //soldier1.onClick.AddListener(playerTeam.spawnSoldier1);
            soldier1.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier1));
            soldier2.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier2));
            soldier3.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier3));
            soldier4.onClick.AddListener(() => playerTeam.spawnUnit(playerTeam.Soldier4));

            magicPrep1.onClick.AddListener(player.prepMagic1);
            magicPrep2.onClick.AddListener(player.prepMagic2);
        }
    }

   
}
