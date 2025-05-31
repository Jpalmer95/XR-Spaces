using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioInteraction : MonoBehaviour
{
    public AudioSource audioSource;
    public float interactionDistance = 2.0f;
    public KeyCode interactionKey = KeyCode.E; // Using the same key as the TV for now

    private Transform playerAvatarTransform;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource.clip == null)
        {
            Debug.LogWarning("RadioInteraction: No AudioClip assigned to the AudioSource. Radio will not play sound.");
        }

        if (audioSource.playOnAwake)
        {
            audioSource.Stop(); // Ensure it doesn't play on start
        }

        GameObject playerObj = GameObject.Find("PlayerAvatar");
        if (playerObj != null)
        {
            playerAvatarTransform = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("RadioInteraction: PlayerAvatar not found. Interaction might not work.");
        }
    }

    void Update()
    {
        if (playerAvatarTransform == null) return;

        float distance = Vector3.Distance(playerAvatarTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            // (Optional UI prompt here)
            // Debug.Log("Near Radio, press E to interact");

            if (Input.GetKeyDown(interactionKey))
            {
                ToggleAudioPlayback();
            }
        }
    }

    public void ToggleAudioPlayback()
    {
        if (audioSource.clip == null)
        {
            Debug.LogWarning("RadioInteraction: Cannot play/pause. No AudioClip assigned.");
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            Debug.Log("Radio Paused");
        }
        else
        {
            audioSource.Play();
            Debug.Log("Radio Playing");
        }
    }
}
