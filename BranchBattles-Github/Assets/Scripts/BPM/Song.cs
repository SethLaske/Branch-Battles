using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Song
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public float BPM { get; set; } // Set to 0 if BPM is not available
    public string TrackId{ get; set; }

    public string ToString()
    {
        return $"{Name} by {Artist} - BPM: {BPM}";
    }
}

public static class SongSaver
{
    private static string filePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "songsData.json");
        }
    }
    
    private static List<Song> cachedSongs = new List<Song>();

    public static void AddSongs(List<Song> argNewSongs)
    {
        cachedSongs = MergeSongs(cachedSongs, argNewSongs);
        
        string json = JsonConvert.SerializeObject(cachedSongs, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static List<Song> GetSongs()
    {
        if (cachedSongs.Count == 0)
        {
            LoadSongs();
        }
        
        return cachedSongs;
    }

    public static void ClearAllSongs()
    {
        cachedSongs.Clear();
        cachedSongs = new List<Song>();
        
        string json = JsonConvert.SerializeObject(cachedSongs, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    private static void LoadSongs()
    {
        cachedSongs.Clear();
        
        if (File.Exists(filePath) == false)
        {
            return;
        }
        
        string json = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return; // Return an empty list if the file is empty
        }

        cachedSongs = JsonConvert.DeserializeObject<List<Song>>(File.ReadAllText(filePath));
    }

    private static List<Song> MergeSongs(List<Song> songs1, List<Song> songs2)
    {
        foreach (var checkedSong in songs2)
        {
            bool songFoundInSongs1 = false;
            
            foreach (var song in songs1)
            {
                if (checkedSong.Name == song.Name && checkedSong.Artist == song.Artist)
                {
                    songFoundInSongs1 = true;
                    song.BPM = checkedSong.BPM;     //Pass in updated BPM if it exists
                }
            }

            if (songFoundInSongs1 == false)
            {
                songs1.Add(checkedSong);
            }
        }

        return songs1;
    }
}