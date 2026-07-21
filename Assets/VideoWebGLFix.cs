using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.Networking; // REQUIRED for UnityWebRequest
using System.Collections.Generic;

[RequireComponent(typeof(VideoPlayer))]
public class VideoWebGLFix : MonoBehaviour
{
    // Store video names instead of VideoClip assets for WebGL
    [Tooltip("List of video filenames (e.g., video1.mp4) in the StreamingAssets folder.")]
    public List<string> videoFileNames = new List<string>();

    private VideoPlayer vp;
    private int currentClipIndex = 0;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.isLooping = false;
        vp.loopPointReached += OnVideoEnd;

        // Start the continuous loop coroutine
        StartCoroutine(PlayVideoPlaylist());
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        PlayNextClip();
    }

    private void PlayNextClip()
    {
        if (videoFileNames.Count == 0)
        {
            Debug.LogError("Video file name list is empty.");
            return;
        }

        // Increment index and wrap around
        currentClipIndex = (currentClipIndex + 1) % videoFileNames.Count;

        // Get the name of the next video file
        string nextFileName = videoFileNames[currentClipIndex];

        // Start playing the video with the WebGL-compatible loading method
        StartCoroutine(LoadAndPlayVideo(nextFileName));
    }

    IEnumerator LoadAndPlayVideo(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        // 1. Assign the URL source to the Video Player
        vp.source = VideoSource.Url;
        vp.url = filePath;

        // 2. Prepare the video (required for loading from URL/StreamingAssets)
        vp.Prepare();

        // 3. Wait until the video is prepared
        yield return new WaitWhile(() => !vp.isPrepared);

        // 4. Play the video
        vp.Play();
        Debug.Log("Playing video: " + fileName);
    }

    IEnumerator PlayVideoPlaylist()
    {
        // Start the first video in the list immediately (index 0)
        string firstFileName = videoFileNames[currentClipIndex];
        yield return StartCoroutine(LoadAndPlayVideo(firstFileName));

        // The rest of the loop is handled by the OnVideoEnd event calling PlayNextClip()
    }

    private void OnDestroy()
    {
        if (vp != null)
        {
            vp.loopPointReached -= OnVideoEnd;
        }
    }
}
