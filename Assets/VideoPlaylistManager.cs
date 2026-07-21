using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic; // Required for List

[RequireComponent(typeof(VideoPlayer))] // Ensures a VideoPlayer is on the GameObject
public class VideoPlaylistManager : MonoBehaviour
{
    // Array to hold your video clips, editable in the Inspector
    public List<VideoClip> videoPlaylist = new List<VideoClip>();

    private VideoPlayer vp;
    private int currentClipIndex = 0;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();

        // IMPORTANT: Uncheck 'Loop' on the VideoPlayer in the Inspector!
        // The script handles the looping logic.
        vp.isLooping = false;

        // Subscribe to the event that fires when a clip finishes playing
        vp.loopPointReached += OnVideoEnd;

        // Start the first video
        PlayNextClip();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // This function is called automatically when the current video finishes.
        PlayNextClip();
    }

    void PlayNextClip()
    {
        // 1. Check if the playlist is empty
        if (videoPlaylist.Count == 0)
        {
            Debug.LogError("Video Playlist is empty! Cannot play.");
            return;
        }

        // 2. Assign the next clip based on the current index
        vp.clip = videoPlaylist[currentClipIndex];

        // 3. Play the video
        vp.Play();

        // 4. Increment the index and wrap around to the start (loop)
        currentClipIndex++;
        if (currentClipIndex >= videoPlaylist.Count)
        {
            currentClipIndex = 0; // Reset to the first clip for the continuous loop
        }

        Debug.Log("Now playing video index: " + currentClipIndex);
    }

    // Best practice to unsubscribe from events when the object is destroyed
    private void OnDestroy()
    {
        if (vp != null)
        {
            vp.loopPointReached -= OnVideoEnd;
        }
    }
}