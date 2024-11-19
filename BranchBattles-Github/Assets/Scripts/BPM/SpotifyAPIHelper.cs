using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class SpotifyAPIHelper
{
    private const string ClientId = "c3282cab588842b7b975eb7f553632da";
    private const string ClientSecret = "d043077442b04bf78d7f17337503747d";
    private string accessToken;

    private IEnumerator GetAccessToken()
    {
        string authUrl = "https://accounts.spotify.com/api/token";
        string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

        UnityWebRequest www = UnityWebRequest.PostWwwForm(authUrl, "");
        www.SetRequestHeader("Authorization", "Basic " + credentials);
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes("grant_type=client_credentials"));
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var json = JObject.Parse(www.downloadHandler.text);
            accessToken = json["access_token"].ToString();
        }
        else
        {
            Debug.LogError($"Error fetching access token: {www.error}\nResponse: {www.downloadHandler.text}");
        }
    }

    public IEnumerator FetchBPMForSongs(List<Song> songs, Action onComplete)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return GetAccessToken();
        }

        // Collect track IDs for batch request
        List<string> trackIds = new List<string>();

        int x = 0;
        
        foreach (var song in songs)
        {
            if (string.IsNullOrEmpty(song.Name) || string.IsNullOrEmpty(song.Artist)) continue;

            string query = $"{song.Name} {song.Artist}";
            string searchUrl = $"https://api.spotify.com/v1/search?q={UnityWebRequest.EscapeURL(query)}&type=track";

            //Debug.Log($"Search for song with URL: {searchUrl}");
            
            UnityWebRequest searchRequest = UnityWebRequest.Get(searchUrl);
            searchRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);
            yield return searchRequest.SendWebRequest();

            if (searchRequest.result == UnityWebRequest.Result.Success)
            {
                var json = JObject.Parse(searchRequest.downloadHandler.text);
                var track = json["tracks"]?["items"]?.First;

                if (track != null)
                {
                    string trackId = track["id"].ToString();
                    song.TrackId = trackId;
                    trackIds.Add(trackId);
                }
            }else
            {
                Debug.LogError($"Error fetching audio features: {searchRequest}");
            }
            
            if (x % (songs.Count / 20) == 0)
            {
                Debug.Log($"Sent {(x * 100 / songs.Count)}% songs from {songs.Count} tracks)");
            }
            x++;
        }

        // Now batch request for audio features (up to 100 track IDs at once)
        const int batchSize = 100;  // Spotify allows up to 100 track IDs per request
        for (int i = 0; i < trackIds.Count; i += batchSize)
        {
            var batch = trackIds.GetRange(i, Mathf.Min(batchSize, trackIds.Count - i));
            yield return FetchAudioFeatures(batch, songs, onComplete);
            yield return new WaitForSeconds(1);  // Add delay between batches to avoid rate limit
        }
    }

    private IEnumerator FetchAudioFeatures(List<string> trackIds, List<Song> songs, Action onComplete)
    {
        // Build the batch request URL with track IDs
        string featuresUrl = $"https://api.spotify.com/v1/audio-features?ids={string.Join(",", trackIds)}";

        UnityWebRequest featuresRequest = UnityWebRequest.Get(featuresUrl);
        featuresRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);

        Debug.Log($"Search for audio with URL: {featuresUrl}");
        
        yield return featuresRequest.SendWebRequest();

        if (featuresRequest.result == UnityWebRequest.Result.Success)
        {
            var json = JObject.Parse(featuresRequest.downloadHandler.text);
            var audioFeaturesArray = json["audio_features"];

            for (int i = 0; i < audioFeaturesArray.Count(); i++)
            {
                var audioFeature = audioFeaturesArray[i];
                var trackId = audioFeature["id"].ToString();
                var bpm = audioFeature["tempo"].ToObject<float>();
                Debug.Log($"Read BPM {bpm}");
                // Find the corresponding song and update BPM
                var song = songs.FirstOrDefault(s => s.TrackId == trackId);
                if (song != null)
                {
                    song.BPM = bpm;
                }
            }
        }
        else
        {
            Debug.LogError($"Error fetching audio features: {featuresRequest}");
        }

        onComplete?.Invoke();
    }
}