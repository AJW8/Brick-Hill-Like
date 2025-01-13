using UnityEngine;
using UnityEngine.UI;

public class SidebarManager : MonoBehaviour
{
    // A singleton instance of the SidebarManager for global access.
    public static SidebarManager Instance;

    // UI fields used for displaying the name, position, rotation, and scale of the selected block.
    public Text objectNameText;
    public InputField positionXInput, positionYInput, positionZInput;
    public InputField rotationXInput, rotationYInput, rotationZInput;
    public InputField scaleXInput, scaleYInput, scaleZInput;

    // Placeholders that indicate if certain scale axes are non-editable.
    [SerializeField] GameObject scaleXPlaceholder, scaleYPlaceholder, scaleZPlaceholder;

    // The block currently being displayed and edited in the sidebar.
    private SelectableBlock currentBlock;

    // Unity's Awake method: Ensures there is only one SidebarManager instance in the scene.
    private void Awake()
    {
        // Initialize the singleton instance or destroy duplicate instances.
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Updates the UI fields in the sidebar with the properties of the specified block.
    public void UpdateSidebar(SelectableBlock block)
    {
        if (block == null) return; // Exit if the block is null (e.g., deselection).

        // Check if the block's scaling is locked for each axis and update placeholders accordingly.
        bool[] scalable = block.GetScalable();
        if (scaleXPlaceholder != null) scaleXPlaceholder.SetActive(!scalable[0]);
        if (scaleYPlaceholder != null) scaleYPlaceholder.SetActive(!scalable[1]);
        if (scaleZPlaceholder != null) scaleZPlaceholder.SetActive(!scalable[2]);

        // Set the current block reference.
        currentBlock = block;

        // Display the block's name in the corresponding UI text field.
        if (objectNameText != null) objectNameText.text = block.GetName();

        // Update position input fields with the block's current position.
        Vector3 position = block.GetPosition();
        if (positionXInput != null) positionXInput.text = position.x.ToString();
        if (positionYInput != null) positionYInput.text = position.y.ToString();
        if (positionZInput != null) positionZInput.text = position.z.ToString();

        // Update rotation input fields with the block's current rotation.
        Vector3 rotation = block.GetRotation();
        if (rotationXInput != null) rotationXInput.text = rotation.x.ToString();
        if (rotationYInput != null) rotationYInput.text = rotation.y.ToString();
        if (rotationZInput != null) rotationZInput.text = rotation.z.ToString();

        // Update scale input fields with the block's current scale.
        Vector3 scale = block.GetScale();
        if (scaleXInput != null) scaleXInput.text = ((int)scale.x).ToString();
        if (scaleYInput != null) scaleYInput.text = ((int)scale.y).ToString();
        if (scaleZInput != null) scaleZInput.text = ((int)scale.z).ToString();
    }

    // Applies changes to the selected block's position based on the input fields.
    public void OnPositionChanged()
    {
        if (currentBlock == null || positionXInput == null || positionYInput == null || positionZInput == null) return;

        float x, y, z; // Local variables for parsing input values.
        
        // Get the block's current position and update values only if parsing succeeds.
        Vector3 currentPosition = currentBlock.GetPosition();
        x = float.TryParse(positionXInput?.text, out x) ? x : currentPosition.x;
        y = float.TryParse(positionYInput?.text, out y) ? y : currentPosition.y;
        z = float.TryParse(positionZInput?.text, out z) ? z : currentPosition.z;

        // Apply the new position values to the block.
        currentBlock.SetPosition(new Vector3(x, y, z));
    }

    // Applies changes to the selected block's rotation based on the input fields.
    public void OnRotationChanged()
    {
        if (currentBlock == null || rotationXInput == null || rotationYInput == null || rotationZInput == null) return;

        float x, y, z;

        // Get the block's current rotation and update values only if parsing succeeds.
        Vector3 currentRotation = currentBlock.GetRotation();
        x = float.TryParse(rotationXInput?.text, out x) ? x : currentRotation.x;
        y = float.TryParse(rotationYInput?.text, out y) ? y : currentRotation.y;
        z = float.TryParse(rotationZInput?.text, out z) ? z : currentRotation.z;

        // Apply the new rotation values to the block.
        currentBlock.SetRotation(new Vector3(x, y, z));
    }

    // Applies changes to the selected block's scale based on the input fields.
    public void OnScaleChanged()
    {
        if (currentBlock == null || scaleXInput == null || scaleYInput == null || scaleZInput == null) return;

        float x, y, z;

        // Determine if each axis is scalable and adjust the scale values accordingly.
        bool[] scalable = currentBlock.GetScalable();
        Vector3 currentScale = currentBlock.GetScale();

        int scaleX = scalable[0] && float.TryParse(scaleXInput?.text, out x) ? (int)Mathf.Max(1, x) : (int)currentScale.x;
        int scaleY = scalable[1] && float.TryParse(scaleYInput?.text, out y) ? (int)Mathf.Max(1, y) : (int)currentScale.y;
        int scaleZ = scalable[2] && float.TryParse(scaleZInput?.text, out z) ? (int)Mathf.Max(1, z) : (int)currentScale.z;

        // Apply the new scale values to the block.
        currentBlock.SetScale(new Vector3(scaleX, scaleY, scaleZ));
    }
}
