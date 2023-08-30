using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The magic effects need to turn the parent off after running the magic
public class Magic : MonoBehaviour
{
    public string magicName;
    public float cooldownTime;
    //public bool playerMagic;
    private TeamInfo team;

    public Sprite buttonUI; //Will use this to automatically assign the magic to the battleUI later
    
    public int soulCost;

    public GameObject mouseTracker;
    public float yHeight;
    public float trackerMoveSpeed;

    public GameObject magicEffect;
    //Will likely need to figure out enums to give it a type, for example lighting is offensive, The World is utility, and maybe a buffing or healing type spell

    [HideInInspector] public MagicButtons magicButton;

    private bool magicAvailable = true;


    /*private void Start()
    {
        magicAvailable = false;
        mouseTracker.SetActive(true);
        magicEffect.SetActive(false);
    }*/
    
    public void TriggerMagic() {
        magicAvailable = true;
        gameObject.SetActive(true);
        mouseTracker.SetActive(true);
        magicEffect.SetActive(false);
    }

    private void Update()
    {
        if (team.Team != 1) {
            return;
        }

        if (magicAvailable == false) {
            return;
        }
            
        TrackMouse();
        
    }

    private void TrackMouse() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseWorldPos.y <= yHeight)
        {
            mouseTracker.transform.position = new Vector3(mouseWorldPos.x, mouseTracker.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0)) {
                ActivateMagic(mouseWorldPos.x);
            }
        }
        else {
            mouseTracker.transform.position = new Vector3(1000, mouseTracker.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
            }
        }
       
    }

    //TODO: Make a function (Coroutine that moves the tracker over to its desired position, giving the player time to respond and see whats going on)
    public void SendMagicToLocation(float startXPosition, float endXPosition) {
        if (magicAvailable == false) return;
        
        TriggerMagic();
        magicAvailable = false;
        mouseTracker.transform.position = new Vector3(startXPosition, mouseTracker.transform.position.y, 0);
        StartCoroutine(MoveTrackerToPoint(endXPosition));
    }

    IEnumerator MoveTrackerToPoint(float endPosition)
    {
        Vector3 finalPosition = new Vector3(endPosition, mouseTracker.transform.position.y, mouseTracker.transform.position.z);
        while(mouseTracker.transform.position.x != endPosition)
        {
            if (LevelManager.gameState == GameState.InGame)
            { 
                mouseTracker.transform.position = Vector3.MoveTowards(mouseTracker.transform.position, finalPosition, trackerMoveSpeed * Time.deltaTime); 
            }

            yield return null;
        }

        ActivateMagic(endPosition);
    }

    public void ActivateMagic(float xPosition)     
    {
        Debug.Log("Magic is being activated");
        magicEffect.SetActive(false);   
        gameObject.SetActive(true);     //Called from scripts where this might not be active
        mouseTracker.SetActive(false);

        Debug.Log("Setting Storm X");
        magicEffect.transform.position = new Vector3(xPosition, 0, 0);
        magicEffect.SetActive(true);
        //magicEffect.SendMessage("UseMagic");
        team.souls -= soulCost;

        if (magicButton != null) {
            magicButton.thisButton.interactable = false;
        }
        
        Invoke("RefreshMagic", cooldownTime);
        //StartCoroutine(ButtonCooldown());
        //magicAvailable = true;
    }

    public void SetTeamInfo(TeamInfo teamInfo) {
        team = teamInfo;
    }


    IEnumerator ButtonCooldown()
    {
        magicButton.thisButton.interactable = false;

        yield return new WaitForSeconds(cooldownTime);

        magicAvailable = true;
        magicButton.thisButton.interactable = true;
    }

    private void RefreshMagic() {
        if (magicButton != null)
        {
            magicButton.thisButton.interactable = true;
        }
        magicAvailable = true;
    }
}
