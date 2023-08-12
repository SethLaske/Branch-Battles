using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public string magicName;
    public float cooldownTime;
    private TeamInfo team;

    public Sprite buttonUI; //Will use this to automatically assign the magic to the battleUI later
    
    public int soulCost;

    public GameObject mouseTracker;
    public float yHeight;

    public GameObject magicEffect;
    //Will likely need to figure out enums to give it a type, for example lighting is offensive, The World is utility, and maybe a buffing or healing type spell

    [HideInInspector] public MagicButtons magicButton;

    private bool magicUsed;


    private void Start()
    {
        magicUsed = false;
        mouseTracker.SetActive(true);
        magicEffect.SetActive(false);
    }
    
    public void TriggerMagic() {
        magicUsed = false;
        gameObject.SetActive(true);
        mouseTracker.SetActive(true);
        magicEffect.SetActive(false);
    }

    private void Update()
    {
        TrackMouse();
        if (Input.GetMouseButtonDown(0) && magicUsed == false) {
            ActivateMagic();
        }
    }

    private void TrackMouse() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseWorldPos.y <= yHeight)
        {
            mouseTracker.transform.position = new Vector3(mouseWorldPos.x, mouseTracker.transform.position.y, 0);
        }
        else {
            mouseTracker.transform.position = new Vector3(1000, mouseTracker.transform.position.y, 0);
        }
    }

    private void ActivateMagic() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseWorldPos.y > yHeight)
        {
            Debug.Log("Mouse is too high, magic cancelled");
            gameObject.SetActive(false);
            return;
        }

        magicEffect.transform.position = new Vector3(mouseTracker.transform.position.x, magicEffect.transform.position.y, 0);
        magicEffect.SetActive(true);
        magicEffect.SendMessage("UseMagic");
        mouseTracker.SetActive(false);
        team.souls -= soulCost;

        magicButton.MagicActivated();
        magicUsed = true;
    }

    public void SetTeamInfo(TeamInfo teamInfo) {
        team = teamInfo;
    }

}
