using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopButton : MonoBehaviour
{
    public int troopNumber;

    // Start is called before the first frame update
    void Start()
    {
        /*if (PlayerPrefs.GetInt("UnlockedTroops") < (troopNumber))
        {
            gameObject.SetActive(false);
        }*/
        if (PlayerInfo.LevelKeys.ContainsKey(troopNumber))
        {
            if (PlayerInfo.TroopKeys[troopNumber] == false)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
