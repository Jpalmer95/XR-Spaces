using UnityEngine;
using UnityEngine.Video; // Required for VideoPlayer

public class TVInteraction : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float interactionDistance = 3.0f; // Distance within which player can interact
    public KeyCode interactionKey = KeyCode.E;

    private Transform playerAvatarTransform; // To store PlayerAvatar's transform

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("TVInteraction: VideoPlayer component not found or assigned. Please assign it in the Inspector or ensure it's on the same GameObject.");
                enabled = false;
                return;
            }
        }

        // Attempt to find the PlayerAvatar by its name. This is a common approach.
        // For more robust solutions, especially with multiple scenes or complex hierarchies,
        // consider using Tags or a GameManager to provide references.
        GameObject playerObj = GameObject.Find("PlayerAvatar");
        if (playerObj != null)
        {
            playerAvatarTransform = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("TVInteraction: PlayerAvatar not found by name. Interaction might not work correctly.");
        }

        // Ensure video doesn't play on start if not already handled by VideoPlayer settings
        if (videoPlayer.playOnAwake)
        {
            videoPlayer.Stop(); // Stop it just in case playOnAwake was true
        }
    }

    void Update()
    {
        if (playerAvatarTransform == null) return; // Player not found, do nothing

        // Check distance between player and TV
        float distance = Vector3.Distance(playerAvatarTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            // (Optional UI prompt would be handled here)
            // Debug.Log("Near TV, press E to interact");

            if (Input.GetKeyDown(interactionKey))
            {
                ToggleVideoPlayback();
            }
        }
    }

    public void ToggleVideoPlayback()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            Debug.Log("TV Paused");
        }
        else
        {
            if (videoPlayer.isPrepared || !string.IsNullOrEmpty(videoPlayer.url)) // Check if ready or has a url
            {
                videoPlayer.Play();
                Debug.Log("TV Playing");
            }
            else
            {
                Debug.LogWarning("TVInteraction: VideoPlayer not prepared or no URL set.");
            }
        }
    }
}
