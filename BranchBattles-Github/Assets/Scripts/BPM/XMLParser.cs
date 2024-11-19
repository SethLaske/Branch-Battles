using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XMLParser : MonoBehaviour
{
    private List<Song> songs = new List<Song>();
    
    public void GetSongs()
    {
        /*if (songs.Count == 0)
        {
            Debug.Log("Getting songs");
            RunParseAndFetchBPMs();
        }
        else
        {
            Debug.Log("Had songs");
            FinalizeSongList();
        }*/

    }
    
    public void RunParseAndFetchBPMs()
    {
        /*songs = XMLSongParser.ParseSongsFromXML("Deprecated.xml");

        while (songs.Count > 50)
        {
            songs.RemoveAt(0);
        }

        SpotifyAPIHelper apiHelper = new SpotifyAPIHelper();
        StartCoroutine(apiHelper.FetchBPMForSongs(songs, FinalizeSongList));*/
    }

    private void FinalizeSongList()
    {
        /*Debug.Log($"Processed {songs.Count} songs:");
        foreach (var song in songs)
        {
            Debug.Log($"{song.ToString()}");
        }*/
    }
}

public class XMLSongParser
{
    public static List<Song> ParseSongsFromXML(string filePath)
    {
        var songs = new List<Song>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        XmlNode tracksNode = xmlDoc.SelectSingleNode("//dict/key[text()='Tracks']/following-sibling::dict");

        if (tracksNode != null)
        {
            foreach (XmlNode trackNode in tracksNode.ChildNodes)
            {
                if (trackNode.Name == "key") // Each track's ID is a "key"
                {
                    XmlNode trackDetails = trackNode.NextSibling;
                    if (trackDetails?.Name == "dict")
                    {
                        string name = null, artist = null;

                        foreach (XmlNode detail in trackDetails.ChildNodes)
                        {
                            if (detail.Name == "key" && detail.InnerText == "Name")
                            {
                                name = detail.NextSibling?.InnerText;
                            }
                            else if (detail.Name == "key" && detail.InnerText == "Artist")
                            {
                                artist = detail.NextSibling?.InnerText;
                            }
                        }

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(artist))
                        {
                            songs.Add(new Song { Name = name, Artist = artist });
                        }
                    }
                }
            }
        }

        return songs;
    }
}
