using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
}
