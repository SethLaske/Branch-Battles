using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScreen : MonoBehaviour
{
    [SerializeField] private string popUpName;

    void Start()
    {
        if (PlayerInfo.PopUpKeys.ContainsKey(popUpName) == false) {
            gameObject.SetActive(false);
            return;
        }

        if (PlayerInfo.PopUpKeys[popUpName] == false)
        {
            PlayerInfo.PopUpKeys[popUpName] = true;
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }

    }

    
}
