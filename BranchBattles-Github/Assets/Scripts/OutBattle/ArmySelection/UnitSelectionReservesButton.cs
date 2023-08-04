using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionReservesButton : MonoBehaviour
{
    public int unitNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInfo.LevelKeys.ContainsKey(unitNumber))
        {
            if (PlayerInfo.TroopKeys[unitNumber] == false)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
