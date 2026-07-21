using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>(); // Uses global UI audio source
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
