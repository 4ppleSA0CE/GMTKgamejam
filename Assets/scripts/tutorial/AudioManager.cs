using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource background;

    public AudioClip backgroundMusic;

    private void Start()
    {
        background.clip = backgroundMusic;
        background.Play();
    }
}

