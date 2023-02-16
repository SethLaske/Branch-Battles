using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Tutorial : MonoBehaviour
{
    public TeamInfo Peasants;
    public TeamInfo Barbarians;


    public bool PassRally;

    public GameObject CameraScreen;
    public GameObject TroopsUI;
    public GameObject TroopsScreen;
    public GameObject PacifistUI;
    public GameObject PacifistScreen;
    public GameObject RallyUI;
    public GameObject RallyScreen;
    public GameObject ChargeUI;
    public GameObject ChargeScreen;
    //public GameObject 
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (PassRally == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            Peasants.RallyFlag.transform.position = new Vector3(Worldpos.x, Peasants.RallyFlag.transform.position.y, 0);
        }
        if (Input.GetMouseButtonDown(0))    //All of the various options for what a player can press (excluding UI buttons)
        {
            
            if (CameraScreen.activeInHierarchy) {
                //CameraScreen.SetActive(false);
                ReadCamera();
            } else if (TroopsScreen.activeInHierarchy)
            {
                //TroopsScreen.SetActive(false);
                ReadTroops();
            }
            else if (PacifistScreen.activeInHierarchy)
            {
                //PacifistScreen.SetActive(false);
                ReadPacifist();
            }
            else if (RallyScreen.activeInHierarchy)
            {
                //RallyScreen.SetActive(false);
                ReadRally();
            }
            else if (ChargeScreen.activeInHierarchy)
            {
                //ChargeScreen.SetActive(false);
                ReadCharge();
            }

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            if (PassRally == true)  //Sets the rally point if rally was already selected
            {
                Peasants.setRallyPoint(Worldpos.x);
                PassRally = false;
            }
            
            else {  
                RaycastHit2D hit = Physics2D.Raycast(Worldpos, Vector2.zero, 10, 1);


                //Debug.Log("Mouse Pressed at " + Worldpos);
                if (hit)
                {
                    Debug.Log("Something Found" + hit.transform.name);
                    Gate gate = hit.transform.gameObject.GetComponent<Gate>();
                    if (gate != null) 
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
                                pacifist.pick.color = Color.magenta;
                            }
                            else if (pacifist.Resource.Equals("Gem"))
                            {
                                pacifist.Resource = "Mine";
                                pacifist.pick.color = Color.yellow;

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

    IEnumerator clickBuffer(GameObject read) {
        yield return new WaitForSeconds(2f);
        read.SetActive(false);
    }

    //Tutorial Specific Scripts
    public void ReadCamera() {
        StartCoroutine(clickBuffer(CameraScreen));
        //CameraScreen.SetActive(false);
        StartCoroutine(ShowTroops());
        Barbarians.spawnPacifist1();
    }

    IEnumerator ShowTroops()
    {
        yield return new WaitForSeconds(3);
        TroopsUI.SetActive(true);
        TroopsScreen.SetActive(true);
        Barbarians.spawnSoldier1();
        //Barbarians.spawnSoldier1();
    }

    public void ReadTroops()
    {
        StartCoroutine(clickBuffer(TroopsScreen));
        //TroopsScreen.SetActive(false);
        StartCoroutine(ShowPacifist());
        
    }

    IEnumerator ShowPacifist (){
        yield return new WaitForSeconds(5);
        Barbarians.Charge();
        PacifistUI.SetActive(true);
        PacifistScreen.SetActive(true);
    }

    public void ReadPacifist()
    {
        StartCoroutine(clickBuffer(PacifistScreen));
        //PacifistScreen.SetActive(false);
        StartCoroutine(ShowRally());
    }

    IEnumerator ShowRally()
    {
        StartCoroutine(EnemyAttacking());
        yield return new WaitForSeconds(5);
        RallyUI.SetActive(true);
        RallyScreen.SetActive(true);
    }

    public void ReadRally()
    {
        StartCoroutine(clickBuffer(RallyScreen)); 
        //RallyScreen.SetActive(false);
        StartCoroutine(ShowCharge());
    }

    IEnumerator ShowCharge()
    {
        yield return new WaitForSeconds(5);
        ChargeUI.SetActive(true);
        ChargeScreen.SetActive(true);
    }

    public void ReadCharge()
    {
        StartCoroutine(clickBuffer(ChargeScreen));
        //ChargeScreen.SetActive(false);
        
    }


    IEnumerator EnemyAttacking() {
        yield return new WaitForSeconds(10);
        Barbarians.spawnSoldier1();
        Barbarians.spawnSoldier2();
        StartCoroutine(EnemyAttacking());
    }
}
