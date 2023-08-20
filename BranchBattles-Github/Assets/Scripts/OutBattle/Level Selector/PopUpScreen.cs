using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScreen : MonoBehaviour
{
    [SerializeField] private string popUpName;


    void Start()
    {
        if (PlayerPrefs.HasKey(popUpName) == false) {
            gameObject.SetActive(false);
            return;
        }

        if (PlayerPrefs.GetInt(popUpName) == -1)
        {
            PlayerPrefs.SetInt(popUpName, 1);
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }

    }

    
}
