using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDisplayView : MonoBehaviour
{
    public TextMeshProUGUI songName;
    public TextMeshProUGUI songArtist;
    public TextMeshProUGUI songBPM;

    public float songBPMValue;
    
    public void Configure(string argName, string argArtist, string argBPM)
    {
        songName.text = argName;
        songArtist.text = argArtist;
        songBPM.text = argBPM;
        
        songBPMValue = float.Parse(argBPM);
    }

    public void Configure(Song argSong)
    {
        songName.text = argSong.Name;
        songArtist.text = argSong.Artist;
        songBPM.text = argSong.BPM.ToString("N0");
        
        songBPMValue = argSong.BPM;
    }
}
