using UnityEngine;

public class DJBoothInteraction : MonoBehaviour
{
    public AudioSource audioSource1; // For Turntable1
    public AudioSource audioSource2; // For Turntable2

    [Range(0.0f, 1.0f)]
    public float crossfadeValue = 0.5f; // 0.0 = full AudioSource1, 1.0 = full AudioSource2

    public float interactionDistance = 3.0f;
    public KeyCode toggleTurntable1Key = KeyCode.Alpha1; // '1' key
    public KeyCode toggleTurntable2Key = KeyCode.Alpha2; // '2' key
    public KeyCode crossfadeLeftKey = KeyCode.Comma;     // ',' or '<' key
    public KeyCode crossfadeRightKey = KeyCode.Period;   // '.' or '>' key
    public float crossfadeSpeed = 0.5f; // How fast crossfadeValue changes per second

    private Transform playerAvatarTransform;

    void Start()
    {
        // Basic validation for AudioSources
        if (audioSource1 == null || audioSource2 == null)
        {
            Debug.LogError("DJBoothInteraction: One or both AudioSources are not assigned. Please assign them in the Inspector.");
            enabled = false;
            return;
        }

        // Ensure clips are handled correctly if not assigned
        if (audioSource1.clip == null) Debug.LogWarning("DJBoothInteraction: AudioSource1 has no AudioClip assigned.");
        if (audioSource2.clip == null) Debug.LogWarning("DJBoothInteraction: AudioSource2 has no AudioClip assigned.");

        // Stop if playing on awake
        if (audioSource1.playOnAwake) audioSource1.Stop();
        if (audioSource2.playOnAwake) audioSource2.Stop();

        // Set initial volumes based on crossfadeValue
        UpdateVolumes();

        GameObject playerObj = GameObject.Find("PlayerAvatar");
        if (playerObj != null)
        {
            playerAvatarTransform = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("DJBoothInteraction: PlayerAvatar not found. Interaction might not work.");
        }
    }

    void Update()
    {
        // Update volumes continuously based on crossfadeValue
        UpdateVolumes();

        if (playerAvatarTransform == null) return;

        float distance = Vector3.Distance(playerAvatarTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            // (Optional UI prompts here)
            // Debug.Log("Near DJ Booth. 1: TT1, 2: TT2, <,>: Crossfade");

            // Toggle Turntable 1
            if (Input.GetKeyDown(toggleTurntable1Key))
            {
                TogglePlayback(audioSource1, "Turntable 1");
            }

            // Toggle Turntable 2
            if (Input.GetKeyDown(toggleTurntable2Key))
            {
                TogglePlayback(audioSource2, "Turntable 2");
            }

            // Crossfade controls
            if (Input.GetKey(crossfadeLeftKey))
            {
                crossfadeValue -= crossfadeSpeed * Time.deltaTime;
                crossfadeValue = Mathf.Clamp01(crossfadeValue);
            }
            if (Input.GetKey(crossfadeRightKey))
            {
                crossfadeValue += crossfadeSpeed * Time.deltaTime;
                crossfadeValue = Mathf.Clamp01(crossfadeValue);
            }
        }
    }

    void TogglePlayback(AudioSource source, string turntableName)
    {
        if (source.clip == null)
        {
            Debug.LogWarning($"DJBoothInteraction: Cannot play/pause {turntableName}. No AudioClip assigned.");
            return;
        }

        if (source.isPlaying)
        {
            source.Pause();
            Debug.Log($"{turntableName} Paused");
        }
        else
        {
            source.Play();
            Debug.Log($"{turntableName} Playing");
        }
    }

    void UpdateVolumes()
    {
        if (audioSource1 != null)
        {
            audioSource1.volume = 1.0f - crossfadeValue;
        }
        if (audioSource2 != null)
        {
            audioSource2.volume = crossfadeValue;
        }
    }
}
