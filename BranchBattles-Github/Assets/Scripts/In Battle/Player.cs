using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Player : MonoBehaviour
{

    private BattleUI battleUI; //Used to be able to access UI elements as needed

    public TeamInfo Peasants;

    private bool PassRally;

    //public bool UseLightning;
    //public bool UseTheWorld;

    public Magic Lightning;
    public Magic TheWorld;

    void Awake()
    {
        if (TheWorld != null) {
            TheWorld.SetTeamInfo(Peasants);
            TheWorld.gameObject.SetActive(false);
        }
        if (Lightning != null)
        {
            Lightning.SetTeamInfo(Peasants);
            Lightning.gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        if (PassRally == true) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Peasants.rallyFlag.transform.position = new Vector3(Worldpos.x, Peasants.rallyFlag.transform.position.y, 0);
        }
        if (Input.GetMouseButtonDown(0))    //All of the various options for what a player can press (excluding UI buttons)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            
            battleUI.UIShadow.SetActive(false);

            if (PassRally == true)  //Sets the rally point if rally was already selected
            {
                Peasants.SetRallyPoint(Worldpos.x);
                PassRally = false;
                battleUI.DelayRallyButton();
            }

            else {  //Checks to see what the player pressed
                RaycastHit2D hit = Physics2D.Raycast(Worldpos, Vector2.zero, 10, 1);


                //Debug.Log("Mouse Pressed at " + Worldpos);
                if (hit)
                {
                    //Debug.Log("Something Found" + hit.transform.name);
                    Gate gate = hit.transform.gameObject.GetComponent<Gate>();
                    if (gate != null) // && gate.team == Peasants.team)  //If its a gate, we want to toggle it
                    {
                        gate.gateSelected();
                    }
                    else {
                        Miner miner = hit.transform.gameObject.GetComponent<Miner>();
                        if (miner != null)
                        {
                            miner.changeResource();
                        }
                    }
                    
                }
            }
            

            
        }
    }

    //Triggered when the Rally Button is pressed
    public void PrepRallyPoint()
    {
        PassRally = true;
        
    }
    
    //Using two different preps to deal with the magic spells as they cant be under the same section
    public void PrepMagic1() {
        if (Lightning.soulCost <= Peasants.souls)
        {
            Lightning.gameObject.SetActive(true);
            battleUI.UIShadow.SetActive(true);
        }
    }

    public void PrepMagic2()
    {
        if (TheWorld.soulCost <= Peasants.souls) {
            TheWorld.gameObject.SetActive(true);
            battleUI.UIShadow.SetActive(true);
        }
    }

    //Both options are available whether winning or losing
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMap()
    {
        Debug.Log("Returning to the map");
        SceneManager.LoadScene("LevelSelect");
    }

    public void SetBattleUI(BattleUI UI) {
        battleUI = UI;
    }

}
