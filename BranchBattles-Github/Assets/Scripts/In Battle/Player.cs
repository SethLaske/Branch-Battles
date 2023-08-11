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

    public Magic magic1;
    public Magic magic2;

    void Awake()
    {
        if (magic1 != null)
        {
            magic1 = Instantiate(magic1, Vector3.zero, Quaternion.identity);
            magic1.SetTeamInfo(Peasants);
            magic1.gameObject.SetActive(false);
        }

        if (magic2 != null)
        {
            magic2 = Instantiate(magic2, Vector3.zero, Quaternion.identity);
            magic2.SetTeamInfo(Peasants);
            magic2.gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame) {
            return;
        }

        if (PassRally == true) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            //Decide if I want to reset its position, disappear it, or merely have it not follow

            if (worldpos.y <= 3) { 
                Peasants.rallyFlag.transform.position = new Vector3(worldpos.x, Peasants.rallyFlag.transform.position.y, 0);
            }
            else
            {
                Peasants.ReloadRallyFlag();
            }
        }
        if (Input.GetMouseButtonDown(0))    //All of the various options for what a player can press (excluding UI buttons)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            
            battleUI.UIShadow.SetActive(false);

            

            if (PassRally == true)  //Sets the rally point if rally was already selected
            {
                PassRally = false;
                battleUI.DelayRallyButton();

                if (worldpos.y <= 3)
                {
                    Peasants.SetRallyPoint(worldpos.x);

                    
                }
                
                
            }

            /*else {  //Checks to see what the player pressed
                RaycastHit2D hit = Physics2D.Raycast(worldpos, Vector2.zero, 10, 1);


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
            }*/
            

            
        }
    }

    //Triggered when the Rally Button is pressed
    public void PrepRallyPoint()
    {
        PassRally = true;
        
    }
    
    //Using two different preps to deal with the magic spells as they cant be under the same section
    public void PrepMagic1() {
        if (magic1.soulCost <= Peasants.souls)
        {
            //magic1.gameObject.SetActive(true);
            magic1.TriggerMagic();
            battleUI.UIShadow.SetActive(true);
        }
    }

    public void PrepMagic2()
    {
        
        if (magic2.soulCost <= Peasants.souls) {
            //magic2.gameObject.SetActive(true);
            magic2.TriggerMagic();
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
