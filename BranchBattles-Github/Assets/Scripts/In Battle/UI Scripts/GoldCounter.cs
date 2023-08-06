using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    private TeamInfo player;
    [Header("Assign in Prefab")]
    public TextMeshProUGUI gold;
    public TextMeshProUGUI troops;
    public TextMeshProUGUI souls;


    public void SetTeamInfo(TeamInfo info) {
        player = info;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        gold.text = "Gold: " + player.gold;
        troops.text = "Troops: " + player.troopCount + "/" + player.maxTroopCount;
        souls.text = "Souls: " + player.souls;
    }
}
