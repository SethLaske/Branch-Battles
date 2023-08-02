using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    private TeamInfo player;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Troops;
    public TextMeshProUGUI Souls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTeamInfo(TeamInfo info) {
        player = info;
    }

    // Update is called once per frame
    void Update()
    {
        Gold.text = "Gold: " + player.gold;
        Troops.text = "Troops: " + player.troopCount + "/" + player.maxTroopCount;
        Souls.text = "Souls: " + player.souls;
    }
}
