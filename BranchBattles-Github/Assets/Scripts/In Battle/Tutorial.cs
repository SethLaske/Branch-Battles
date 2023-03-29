using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Tutorial : MonoBehaviour
{
    public TeamInfo Peasants;
    public TeamInfo Barbarians;

    public Unit miner;
    public Unit fighter;
    public Unit spear;

    /*
    [Header ("Tutorial Explanation Screens")]
    public GameObject CameraScreen;
    public GameObject SoldiersUI;
    public GameObject SoldiersScreen;
    public GameObject PacifistUI;
    public GameObject PacifistScreen;
    public GameObject ArchersUI;
    public GameObject ArchersScreen;
    public GameObject RallyUI;
    public GameObject RallyScreen;
    public GameObject ChargeUI;
    public GameObject ChargeScreen;
    //public GameObject */
    
    void Start()
    {
        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;
    }


    void Update()
    {
        
    }

    
}
