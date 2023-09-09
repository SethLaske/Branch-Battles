using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnButtonPairs
{
    public Button button;
    public Unit unit;
    //public Dictionary<Button, Unit> unitButtonPairs;
}

public class PlayerControlledEnemy : MonoBehaviour
{
    public TeamInfo enemyTeamInfo;

    public List<SpawnButtonPairs> Pairs;
    public List<SpawnButtonPairs> playerPairs;
    
    public TextMeshProUGUI gold;
    public TextMeshProUGUI troops;

    private bool PassRally = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var unitButton in Pairs) { 
            InitTrainButton(unitButton.button, unitButton.unit, enemyTeamInfo);
        }

        foreach (var unitButton in playerPairs)
        {
            InitTrainButton(unitButton.button, unitButton.unit, enemyTeamInfo.Opponent);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        gold.text = "Gold: " + enemyTeamInfo.gold;
        troops.text = "Troops: " + enemyTeamInfo.troopCount + "/" + enemyTeamInfo.maxTroopCount;

        if (PassRally == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            //Decide if I want to reset its position, disappear it, or merely have it not follow

            if (worldpos.y <= 3)
            {
                enemyTeamInfo.rallyFlag.transform.position = new Vector3(worldpos.x, enemyTeamInfo.rallyFlag.transform.position.y, enemyTeamInfo.rallyFlag.transform.position.z);
            }
            else
            {
                enemyTeamInfo.ReloadRallyFlag();
            }
        }
        if (Input.GetMouseButtonDown(0))    //All of the various options for what a player can press (excluding UI buttons)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            //battleUI.UIShadow.SetActive(false);



            if (PassRally == true)  //Sets the rally point if rally was already selected
            {
                PassRally = false;
                //battleUI.DelayRallyButton();

                if (worldpos.y <= 3)
                {
                    enemyTeamInfo.SetRallyPoint(worldpos.x);


                }


            }

            
        }
    }
    public void PrepEnemyRallyPoint()
    {
        PassRally = true;

    }

    private void InitTrainButton(Button button, Unit unit, TeamInfo team)
    {
        if (unit == null)
        {
            
            button.gameObject.SetActive(false);
        }
        else
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => team.TrainUnit(unit));
           
            UnitButtons script = button.GetComponent<UnitButtons>();
            script.SetUnitType(unit);
        }

    }
}
