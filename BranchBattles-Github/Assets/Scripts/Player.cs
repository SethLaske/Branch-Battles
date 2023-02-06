using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Player : MonoBehaviour
{
    public TeamInfo Peasants;

    public bool PassRally;
    public bool UseLightning;
    public bool UseTheWorld;

    public Magic Lightning;
    public Magic TheWorld;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //All of the various options for what a player can press (excluding UI buttons)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            if (PassRally == true)  //Sets the rally point if rally was already selected
            {
                Peasants.setRallyPoint(Worldpos.x);
                PassRally = false;
            }
            else if (UseLightning == true)  //Sets the rally point if rally was already selected
            {
                Peasants.useMagic(Lightning, Worldpos.x);
                UseLightning = false;
            }
            else if (UseTheWorld == true)  //Sets the rally point if rally was already selected
            {
                Peasants.useMagic(TheWorld, Worldpos.x);
                UseTheWorld = false;
            }
            else {  //Checks to see what the player pressed
                RaycastHit2D hit = Physics2D.Raycast(Worldpos, Vector2.zero, 10, 1);


                //Debug.Log("Mouse Pressed at " + Worldpos);
                if (hit)
                {
                    Debug.Log("Something Found" + hit.transform.name);
                    Gate gate = hit.transform.gameObject.GetComponent<Gate>();
                    if (gate != null) // && gate.team == Peasants.team)  //If its a gate, we want to toggle it
                    {
                        gate.gateSelected();
                    }
                    else {
                        Pacifist pacifist = hit.transform.gameObject.GetComponent<Pacifist>();
                        if (pacifist != null && !pacifist.Full)
                        {
                            if (pacifist.Resource.Equals("Mine"))
                            {
                                pacifist.Resource = "Gem";
                            }
                            else if (pacifist.Resource.Equals("Gem"))
                            {
                                pacifist.Resource = "Mine";

                            }
                        }
                    }
                    
                }
            }
            

            
        }
    }

    //Triggered when the Rally Button is pressed
    public void prepRallyPoint()
    {
        PassRally = true;
    }

    public void prepLightningSpell()
    {
        UseLightning = true;
    }

    public void prepTheWorld()
    {
        UseTheWorld = true;
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
}
