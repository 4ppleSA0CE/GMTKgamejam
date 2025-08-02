using UnityEngine;

public class footstepsound : MonoBehaviour
{
    public GameObject footstep;
    private AudioSource footstepAudio;

    void Start()
    {
        footstepAudio = footstep.GetComponent<AudioSource>();
        // Optionally, ensure it doesn't play on awake
        footstepAudio.playOnAwake = false;
    }

    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.loop = true;
                footstepAudio.Play();
            }
        }
        else
        {
            if (footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }
    }
}