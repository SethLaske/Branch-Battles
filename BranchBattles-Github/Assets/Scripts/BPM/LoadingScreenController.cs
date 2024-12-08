using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public static LoadingScreenController instance;
    
    public Slider loadingBar;
    public AddingSongProgressData addingSongProgressData;

    private void Awake()
    {
        instance = this;
    }

    //Really just to fuck with it
    public void UpdateProgress()
    {
        loadingBar.value = addingSongProgressData.parsingProgress * .05f +
                           addingSongProgressData.sortingProgress * .05f +
                           addingSongProgressData.trackIDProgress * .8f +
                           addingSongProgressData.batchProgress * .1f;
    }

    public void ResetProgress()
    {
        addingSongProgressData.parsingProgress = 0;
        addingSongProgressData.sortingProgress = 0;
        addingSongProgressData.trackIDProgress = 0;
        addingSongProgressData.batchProgress = 0;
        
        UpdateProgress();
    }
}

public struct AddingSongProgressData
{
    public float parsingProgress;
    public float sortingProgress;
    public float trackIDProgress;
    public float batchProgress;
    
}
