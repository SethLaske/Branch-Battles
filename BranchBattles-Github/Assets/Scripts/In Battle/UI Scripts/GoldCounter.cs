using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    public TeamInfo player;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Troops;
    public TextMeshProUGUI Souls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gold.text = "Gold: " + player.Gold;
        Troops.text = "Troops: " + player.TroopCount + "/" + player.TroopMax;
        Souls.text = "Souls: " + player.Souls;
    }
}
