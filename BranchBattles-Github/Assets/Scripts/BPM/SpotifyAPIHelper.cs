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

        WWWForm form = new WWWForm();
        form.AddField("grant_type", "client_credentials");

        UnityWebRequest www = UnityWebRequest.Post(authUrl, form);
        www.SetRequestHeader("Authorization", "Basic " + credentials);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var json = JObject.Parse(www.downloadHandler.text);
            accessToken = json["access_token"]?.ToString();
            Debug.Log($"Access Token Retrieved: {accessToken}");
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

            if (string.IsNullOrEmpty(song.TrackId) == false)
            {
                trackIds.Add(song.TrackId);
                continue;
            }

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
            
            if ((songs.Count > 20) && (x % (songs.Count / 20) == 0))
            {
                Debug.Log($"Sent {(x * 100 / songs.Count)}% songs from {songs.Count} tracks)");
            }
            
            LoadingScreenController.instance.addingSongProgressData.trackIDProgress = (float)x/songs.Count;
            LoadingScreenController.instance.UpdateProgress();
            
            x++;
        }

        LoadingScreenController.instance.addingSongProgressData.trackIDProgress = 1;
        LoadingScreenController.instance.UpdateProgress();
        
        if (trackIds.Count <= 0)
        {
            onComplete?.Invoke();
            yield break;
        }

        string testTrackId = "6rqhFgbbKwnb9MLmUQDhG6";  // Replace with a valid ID
        string testUrl = $"https://api.spotify.com/v1/audio-features/{testTrackId}";
        UnityWebRequest testRequest = UnityWebRequest.Get(testUrl);
        testRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);
        yield return testRequest.SendWebRequest();

        if (testRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success: {testRequest.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {testRequest.error}");
        }
        
        // Now batch request for audio features (up to 100 track IDs at once)
        const int batchSize = 50;  // Spotify allows up to 100 track IDs per request
        
        int batchCount = 1 + (songs.Count - 1)/batchSize;
        CoroutineCounter batchCounter = new CoroutineCounter(batchCount, onComplete);
        
        for (int i = 0; i < trackIds.Count; i += batchSize)
        {
            yield return new WaitForSeconds(1);  // Add delay between batches to avoid rate limit
            var batch = trackIds.GetRange(i, Mathf.Min(batchSize, trackIds.Count - i));
            yield return FetchAudioFeatures(batch, songs, batchCounter.OnCoroutineFinished);
            
            LoadingScreenController.instance.addingSongProgressData.batchProgress = (float)i/songs.Count;
            LoadingScreenController.instance.UpdateProgress();
            
        }
    }

    private IEnumerator FetchAudioFeatures(List<string> trackIds, List<Song> songs, Action onComplete)
    {
        const int BatchSize = 100;
        var trackIdBatches = SplitIntoBatches(trackIds, BatchSize);

        foreach (var batch in trackIdBatches)
        {
            string featuresUrl = $"https://api.spotify.com/v1/audio-features?ids={string.Join(",", batch)}";

            UnityWebRequest featuresRequest = UnityWebRequest.Get(featuresUrl);
            featuresRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);

            Debug.Log($"Fetching audio features for batch: {string.Join(",", batch)}");

            yield return featuresRequest.SendWebRequest();

            if (featuresRequest.result == UnityWebRequest.Result.Success)
            {
                var json = JObject.Parse(featuresRequest.downloadHandler.text);
                var audioFeaturesArray = json["audio_features"];

                for (int i = 0; i < audioFeaturesArray.Count(); i++)
                {
                    var audioFeature = audioFeaturesArray[i];
                    var trackId = audioFeature?["id"]?.ToString();
                    var bpm = audioFeature?["tempo"]?.ToObject<float>();

                    if (!string.IsNullOrEmpty(trackId) && bpm.HasValue)
                    {
                        var song = songs.FirstOrDefault(s => s.TrackId == trackId);
                        if (song != null)
                        {
                            song.BPM = bpm.Value;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Error fetching audio features for batch: {featuresRequest.error}");
                Debug.LogError($"Response: {featuresRequest.downloadHandler.text}");
            }
        }

        onComplete?.Invoke();
    }

    // Helper: Split list into batches
    private List<List<string>> SplitIntoBatches(List<string> items, int batchSize)
    {
        var batches = new List<List<string>>();
        for (int i = 0; i < items.Count; i += batchSize)
        {
            batches.Add(items.GetRange(i, Math.Min(batchSize, items.Count - i)));
        }
        return batches;
    }

}