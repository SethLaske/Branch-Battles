using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> popUpScreens;
    void Awake()
    {
        foreach (GameObject popUp in popUpScreens) {
            popUp.SetActive(true);
        }
    }

    
}
