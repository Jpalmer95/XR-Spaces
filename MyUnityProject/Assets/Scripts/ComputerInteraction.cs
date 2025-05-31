using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ComputerInteraction : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode closeBrowserKey = KeyCode.Escape;

    [Header("Browser UI Elements")]
    public GameObject browserCanvas;    // The main canvas for the browser UI
    public Text urlDisplayText;         // Text to show current general "URL" or main message
    // public Button closeButton; // Already handled by Escape key primarily

    [Header("Hugging Face UI Elements")]
    public Button openHFSpacesButton;
    public InputField hfSearchInputField;
    public Button hfSearchButton;
    public Text hfStatusText; // To display HF specific messages like "Searching..." or "Displaying Space..."

    public string defaultUrl = "https://www.google.com";
    public string hfSpacesHomepageUrl = "https://huggingface.co/spaces";

    private Transform playerAvatarTransform;
    private bool isBrowserOpen = false;

    void Start()
    {
        GameObject playerObj = GameObject.Find("PlayerAvatar");
        if (playerObj != null) playerAvatarTransform = playerObj.transform;
        else Debug.LogWarning("ComputerInteraction: PlayerAvatar not found.");

        if (browserCanvas == null)
        {
            Debug.LogError("ComputerInteraction: BrowserCanvas not assigned.");
            enabled = false;
            return;
        }

        // Setup UI listeners for HF functionality
        // These listeners are now set up in the scene YAML by directly referencing the methods
        // if (openHFSpacesButton != null) openHFSpacesButton.onClick.AddListener(HandleOpenHFSpacesButtonClick);
        // else Debug.LogWarning("ComputerInteraction: OpenHFSpacesButton not assigned.");

        // if (hfSearchButton != null) hfSearchButton.onClick.AddListener(HandleSearchHFSpacesButtonClick);
        // else Debug.LogWarning("ComputerInteraction: HFSearchButton not assigned.");

        if (hfStatusText != null) hfStatusText.text = ""; // Clear HF status initially
        else Debug.LogWarning("ComputerInteraction: HFStatusText not assigned.");


        browserCanvas.SetActive(false);
        isBrowserOpen = false;
        UpdateGeneralURLDisplay(defaultUrl);
    }

    void Update()
    {
        if (isBrowserOpen)
        {
            if (Input.GetKeyDown(closeBrowserKey)) CloseBrowser();
        }
        else
        {
            if (playerAvatarTransform == null) return;
            float distance = Vector3.Distance(playerAvatarTransform.position, transform.position);
            if (distance <= interactionDistance && Input.GetKeyDown(interactionKey))
            {
                OpenBrowser();
            }
        }
    }

    public void OpenBrowser()
    {
        browserCanvas.SetActive(true);
        isBrowserOpen = true;
        UpdateGeneralURLDisplay($"Browser Active. Default: {defaultUrl}");
        if(hfStatusText != null) hfStatusText.text = "Select an option or search Hugging Face Spaces.";
        Debug.Log("ComputerInteraction: Browser opened.");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBrowser()
    {
        browserCanvas.SetActive(false);
        isBrowserOpen = false;
        Debug.Log("ComputerInteraction: Browser closed.");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateGeneralURLDisplay(string text)
    {
        if (urlDisplayText != null) urlDisplayText.text = text;
    }

    private void UpdateHFStatusDisplay(string text)
    {
        if (hfStatusText != null) hfStatusText.text = text;
    }

    // --- Hugging Face Specific Methods ---
    // These methods are now public and will be called by UI Button OnClick events configured in the scene.
    public void HandleOpenHFSpacesButtonClick()
    {
        if (!isBrowserOpen) return;
        Debug.Log("ComputerInteraction: 'Open Hugging Face Spaces' button clicked.");
        UpdateGeneralURLDisplay($"Current View: Hugging Face Spaces Homepage ({hfSpacesHomepageUrl})");
        UpdateHFStatusDisplay("Welcome to Hugging Face Spaces! (Simulated)\nUse search to find a Space.");
    }

    public void HandleSearchHFSpacesButtonClick()
    {
        if (!isBrowserOpen || hfSearchInputField == null) return;

        string searchQuery = hfSearchInputField.text;
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            UpdateHFStatusDisplay("Please enter a search query for Hugging Face Spaces.");
            return;
        }

        Debug.Log($"ComputerInteraction: HF Spaces Search for: {searchQuery}");
        UpdateGeneralURLDisplay($"Searching Spaces for: {searchQuery}...");
        UpdateHFStatusDisplay($"Displaying simulated results for: '{searchQuery}'.\n(Simulated - No actual API call or results list).");

        if (searchQuery.ToLower().Contains("bert"))
        {
            SimulateSelectingSpace("BERT Question Answering");
        } else if (searchQuery.ToLower().Contains("diffusion")) {
            SimulateSelectingSpace("Stable Diffusion Demo");
        }
    }

    private void SimulateSelectingSpace(string spaceName)
    {
        UpdateGeneralURLDisplay($"Current View: Space - {spaceName} (Simulated)");
        UpdateHFStatusDisplay($"Welcome to the '{spaceName}' Space!\n(This is a simulated view of the Space running).");
    }

    public void HandleCloseButtonClick()
    {
        CloseBrowser();
    }
}
