using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    private TeamInfo team;

    
    public int soulCost;

    public GameObject mouseTracker;
    public float yHeight;

    public GameObject magicEffect;
    //Will likely need to figure out enums to give it a type, for example lighting is offensive, The World is utility, and maybe a buffing or healing type spell



    private void Update()
    {
        TrackMouse();
        if (Input.GetMouseButtonDown(0)) {
            ActivateMagic();
            this.gameObject.SetActive(false);
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
            return;
        }

        Instantiate(magicEffect, new Vector3(mouseTracker.transform.position.x, 0, 0), Quaternion.identity).SetActive(true);
        team.souls -= soulCost;
    }

    public void SetTeamInfo(TeamInfo teamInfo) {
        team = teamInfo;
    }

}
