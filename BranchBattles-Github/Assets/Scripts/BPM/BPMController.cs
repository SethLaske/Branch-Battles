using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BPMController : MonoBehaviour
{
    private string xmlFilePath = "";
    private float strideLength = 0;
    private float minBPM = 0;
    private float maxBPM = 0;
    private bool allowBPMVariations = false;

    //private List<Song> shownSongs = new List<Song>();
    private List<SongDisplayView> displayedSongs = new List<SongDisplayView>();
    
    [SerializeField]
    private SongDisplayView displayedSongPrefab = null;
    [SerializeField]
    private Transform displayedSongsParent = null;

    [Header("Input Fields")] 
    [SerializeField] private Button addSongs = null;
    [SerializeField] private Button removeSongs = null;
    [SerializeField] private Button getSongsInRange = null;
    
    [SerializeField] private TMP_InputField filePathField = null;
    [SerializeField] private TMP_InputField strideLengthField = null;
    [SerializeField] private TMP_InputField minBPMField = null;
    [SerializeField] private TMP_InputField maxBPMField = null;
    
    [SerializeField] private Toggle allowBPMVariationsToggle = null;
    
    private void Start()
    {
        DisplaySongs(SongSaver.GetSongs());
        
        HandleButtons();
    }

    private void HandleButtons()
    {
        addSongs.onClick.AddListener(OnAddSongsPressed);
        removeSongs.onClick.AddListener(OnRemoveSongsPressed);
        getSongsInRange.onClick.AddListener(OnGetSongsInRangePressed);
        
        filePathField.onValueChanged.AddListener(OnSongsFileNameUpdated);
        strideLengthField.onValueChanged.AddListener(OnStrideLengthUpdated);
        minBPMField.onValueChanged.AddListener(OnMinBPMUpdated);
        maxBPMField.onValueChanged.AddListener(OnMaxBPMUpdated);
        
        allowBPMVariationsToggle.onValueChanged.AddListener(OnAllowVariationsToggled);
        allowBPMVariationsToggle.isOn = allowBPMVariations;
    }

    private void DisplaySongs(List<Song> songs)
    {
        foreach (var songObject in displayedSongs)
        {
            Destroy(songObject.gameObject);
        }
        
        displayedSongs.Clear();
        
        foreach (var song in songs)
        {
            SongDisplayView newView = Instantiate(displayedSongPrefab, displayedSongsParent);
            newView.Configure(song);
            displayedSongs.Add(newView);
        }
    }

    public const string currentPath = @"C:\Users\small\Downloads\Alyssa_Music.xml";

    public void OnSongsFileNameUpdated(string newPath)
    {
        xmlFilePath = newPath;
    }

    public void OnAddSongsPressed()
    {
        List<Song> newSongs = XMLSongParser.ParseSongsFromXML(currentPath);
        
        SpotifyAPIHelper apiHelper = new SpotifyAPIHelper();
        StartCoroutine(apiHelper.FetchBPMForSongs(newSongs, delegate
        {
            Debug.Log("Successfully fetched BPM for songs");
            SongSaver.AddSongs(newSongs);
            DisplaySongs(SongSaver.GetSongs());
        }));
    }

    public void OnStrideLengthUpdated(string argStrideLength)
    {
        strideLength = float.Parse(argStrideLength);
        strideLengthField.text = strideLength.ToString();
    }

    public void OnMinBPMUpdated(string argMinBPM)
    {
        minBPM = float.Parse(argMinBPM);
        minBPMField.text = minBPM.ToString();
    }
    
    public void OnMaxBPMUpdated(string argMaxBPM)
    {
        maxBPM = float.Parse(argMaxBPM);
        maxBPMField.text = maxBPM.ToString();
    }
    
    public void OnRemoveSongsPressed()
    {
        SongSaver.ClearAllSongs();
        DisplaySongs(SongSaver.GetSongs());
    }

    public void OnAllowVariationsToggled(bool argAllow)
    {
        allowBPMVariations = argAllow;
    }

    public void OnGetSongsInRangePressed()
    {
        float bpm = 0;
        
        foreach (SongDisplayView song in displayedSongs)
        {
            bpm = song.songBPMValue;

            bool isSongInRange = bpm >= minBPM && bpm <= maxBPM;

            if (allowBPMVariations)
            {
                isSongInRange |= (bpm >= minBPM/2 && bpm <= maxBPM/2);
                isSongInRange |= (bpm >= minBPM*2 && bpm <= maxBPM*2);
            }

            song.gameObject.SetActive(isSongInRange);
        }
    }
}
