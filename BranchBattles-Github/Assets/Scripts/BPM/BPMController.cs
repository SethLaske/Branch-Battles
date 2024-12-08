using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms; // Requires System.Windows.Forms.dll
using System.IO;
using Button = UnityEngine.UI.Button;

public class BPMController : MonoBehaviour
{
    private string xmlFilePath = "";
    private float strideLength = 0;
    private float pace = 0;     //Minutes per mile 6:00 pace = 6
    private float bpmRange = 0;
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
    [SerializeField] private Button selectFilePath = null;
    [SerializeField] private Button removeSongs = null;
    [SerializeField] private Button getSongsInRange = null;
    
    [SerializeField] private TextMeshProUGUI fileNameText = null;
    
    [SerializeField] private TMP_InputField filePathField = null;
    [SerializeField] private TMP_InputField strideLengthField = null;
    [SerializeField] private TMP_InputField paceField = null;
    [SerializeField] private TMP_InputField bpmRangeField = null;
    [SerializeField] private TMP_InputField minBPMField = null;
    [SerializeField] private TMP_InputField maxBPMField = null;
    
    [SerializeField] private Toggle allowBPMVariationsToggle = null;
    
    private void Start()
    {
        DisplaySongs(SongSaver.GetSongs());
        
        HandleButtons();
        
        LoadingScreenController.instance.gameObject.SetActive(false);
    }

    private void HandleButtons()
    {
        addSongs.onClick.AddListener(OnAddSongsPressed);
        selectFilePath.onClick.AddListener(OnSelectFileNamePressed);
        removeSongs.onClick.AddListener(OnRemoveSongsPressed);
        getSongsInRange.onClick.AddListener(OnGetSongsInRangePressed);
        
        filePathField.onEndEdit.AddListener(OnSongsFileNameUpdated);
        strideLengthField.onEndEdit.AddListener(OnStrideLengthUpdated);
        paceField.onEndEdit.AddListener(OnPaceUpdated);
        bpmRangeField.onEndEdit.AddListener(OnBPMRangeUpdated);
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
            newView.gameObject.name = song.Name;
            newView.Configure(song);
            displayedSongs.Add(newView);
        }
    }

    // const string currentPath = @"C:\Users\small\Downloads\Alyssa_Music.xml";

    public void OnSongsFileNameUpdated(string newPath)
    {
        xmlFilePath = newPath;
    }
    
    public void OnSelectFileNamePressed()
    {
        // Open a file dialog to select a file
        OpenFileDialog fileDialog = new OpenFileDialog
        {
            Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Select an XML File"
        };

        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFilePath = fileDialog.FileName;
            Debug.Log($"Selected file: {selectedFilePath}");

            // Check if the file is XML
            if (Path.GetExtension(selectedFilePath).Equals(".xml", StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("The selected file is an XML.");
                fileNameText.text = selectedFilePath;
                xmlFilePath = selectedFilePath;
            }
            else
            {
                Debug.LogWarning("The selected file is not an XML.");
                fileNameText.text = "XML FILE ALYSSA!";
            }
        }
    }

    public void OnAddSongsPressed()
    {
        LoadingScreenController.instance.gameObject.SetActive(true);
        LoadingScreenController.instance.ResetProgress();
        
        List<Song> songsFromXML = XMLSongParser.ParseSongsFromXML(xmlFilePath);
        
        List<Song> songsToGetBPM = SongSaver.GetNonRecordedSongsFromList(songsFromXML);
        
        SpotifyAPIHelper apiHelper = new SpotifyAPIHelper();
        StartCoroutine(apiHelper.FetchBPMForSongs(songsToGetBPM, delegate
        {
            Debug.Log("Successfully fetched BPM for songs");
            SongSaver.AddSongs(songsToGetBPM);
            DisplaySongs(SongSaver.GetSongs());
            LoadingScreenController.instance.gameObject.SetActive(false);
        }));
    }

    public void OnStrideLengthUpdated(string argStrideLength)
    {
        strideLength = float.Parse(argStrideLength);
        strideLength = Mathf.Max(strideLength, 0);
        
        strideLengthField.text = strideLength.ToString("N2");

        TryUpdateBPMFields();
    }
    
    public void OnPaceUpdated(string argPace)
    {
        if (argPace.Contains(":"))
        {
            var parts = argPace.Split(':');
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            pace = minutes + (seconds / 60f);
        }
        else
        {
            pace = float.Parse(argPace);
        }
        pace = Mathf.Max(pace, 0);
        
        int stringMinutes = (int)Math.Floor(pace);
        int atringSeconds = (int)Math.Round((pace - stringMinutes) * 60);
        
        paceField.text = $"{stringMinutes}:{atringSeconds:D2}";

        TryUpdateBPMFields();
    }
    
    public void OnBPMRangeUpdated(string argRange)
    {
        bpmRange = float.Parse(argRange);
        bpmRange = Mathf.Max(bpmRange, 0);
        
        bpmRangeField.text = bpmRange.ToString("N2");

        TryUpdateBPMFields();
    }

    private void TryUpdateBPMFields()
    {
        if (pace <= 0 || strideLength <= 0 || bpmRange < 0)
        {
            return;
        }
        
        float stepsPerMinute = 5280 / (pace * strideLength);
        
        minBPM = stepsPerMinute - bpmRange/2;
        maxBPM = stepsPerMinute + bpmRange/2;
        
        minBPMField.text = minBPM.ToString("N2");
        maxBPMField.text = maxBPM.ToString("N2");
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
