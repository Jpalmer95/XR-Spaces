using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // For List

public class RoomCustomizationController : MonoBehaviour
{
    [Header("Wall Settings")]
    public MeshRenderer[] wallRenderers; // Assign all four wall MeshRenderers here
    public List<Color> wallColors = new List<Color>();
    public List<string> wallColorNames = new List<string>(); // Optional names for colors

    private int currentColorIndex = 0;

    [Header("UI Settings")]
    public GameObject customizationCanvas; // The main canvas for color selection UI
    public Text currentColorNameText;   // Text to display current color name

    void Start()
    {
        if (wallRenderers == null || wallRenderers.Length == 0)
        {
            Debug.LogError("RoomCustomizationController: Wall Renderers not assigned or empty.");
            enabled = false;
            return;
        }

        if (customizationCanvas == null)
        {
            Debug.LogError("RoomCustomizationController: CustomizationCanvas not assigned.");
            enabled = false;
            return;
        }

        // Populate default colors if list is empty
        if (wallColors.Count == 0)
        {
            wallColors.Add(Color.white); wallColorNames.Add("White");
            wallColors.Add(new Color(0.8f, 0.8f, 0.95f)); wallColorNames.Add("Original Light Blue"); // Default from scene
            wallColors.Add(new Color(0.9f, 0.7f, 0.7f)); wallColorNames.Add("Soft Red");
            wallColors.Add(new Color(0.7f, 0.9f, 0.7f)); wallColorNames.Add("Soft Green");
            wallColors.Add(new Color(0.7f, 0.7f, 0.9f)); wallColorNames.Add("Soft Blue");
            wallColors.Add(new Color(0.9f, 0.9f, 0.7f)); wallColorNames.Add("Soft Yellow");
        }

        // Set initial wall color (optional, could read from one of the walls or use a default)
        // For now, assume the first color in the list or the existing material color is fine.
        // We'll update the UI text if available.
        ApplyColorToWalls(wallColors[currentColorIndex]);
        UpdateColorNameText();

        customizationCanvas.SetActive(false); // Start with UI hidden
    }

    public void ChangeWallColor()
    {
        currentColorIndex = (currentColorIndex + 1) % wallColors.Count;
        ApplyColorToWalls(wallColors[currentColorIndex]);
        UpdateColorNameText();
        Debug.Log("Wall color changed to: " + (wallColorNames.Count > currentColorIndex ? wallColorNames[currentColorIndex] : "Custom"));
    }

    private void ApplyColorToWalls(Color newColor)
    {
        foreach (MeshRenderer renderer in wallRenderers)
        {
            if (renderer != null)
            {
                // This will instance the material if it's shared by multiple renderers
                // and only change the color for this specific renderer's instance.
                // If all walls share the exact same material asset and you want them all
                // to change together, this is fine. If they need to be distinct even before
                // this script runs, they should have separate material assets assigned.
                renderer.material.color = newColor;
            }
        }
    }

    private void UpdateColorNameText()
    {
        if (currentColorNameText != null && wallColorNames.Count > currentColorIndex)
        {
            currentColorNameText.text = "Color: " + wallColorNames[currentColorIndex];
        }
        else if (currentColorNameText != null)
        {
            currentColorNameText.text = "Color: Custom";
        }
    }

    public void ToggleCustomizationUI()
    {
        bool isActive = !customizationCanvas.activeSelf;
        customizationCanvas.SetActive(isActive);
        Debug.Log("Customization UI " + (isActive ? "Enabled" : "Disabled"));

        // Optional: Manage cursor lock state if this UI needs mouse interaction
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Assuming game returns to a state where cursor should be locked
            // This might conflict with other UIs (like the computer browser) if not managed carefully
            // For now, let's assume the player movement script handles locking on its own when no UI is active.
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }
    }
}
