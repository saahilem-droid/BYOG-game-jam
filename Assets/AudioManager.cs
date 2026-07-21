using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public string targetSceneName = "mainmenu";

    public AudioClip Lofi_sound;
    public AudioClip LofiSpeed_sound;
    private void Start()
    {
        musicSource.clip = Lofi_sound;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == targetSceneName)
        {
            musicSource.loop = false;
        }

        musicSource.Play();

        if (currentScene != targetSceneName)
        {
            StartCoroutine(PlayNextTrack(LofiSpeed_sound));
        }

        

        IEnumerator PlayNextTrack(AudioClip nextTrack)
        {
            // Wait until the current track finishes playing.
            // This checks every frame.
            yield return new WaitWhile(() => musicSource.isPlaying);

            // Code execution resumes here when the current song stops

            // 1. Assign the new track
            musicSource.clip = nextTrack;

            // 2. Start playing the new track
            musicSource.Play();

            Debug.Log("First track finished. Now playing " + nextTrack.name);
        }
    }
}


