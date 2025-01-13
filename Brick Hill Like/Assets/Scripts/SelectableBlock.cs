using UnityEngine;

public class SelectableBlock : MonoBehaviour
{
    // Fields
    [SerializeField] private Color selectedColor;
    [SerializeField] private bool scalableX, scalableY, scalableZ, isGroup;

    [SerializeField] private SidebarManager sidebarManager;
    [SerializeField] private ColorManager colorManager;

    private Renderer blockRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isCustomColor = false;
    public static SelectableBlock SelectedBlock { get; set; }
    private static ObjectManager objectManager;

    
    void Start()
    {
        if (!isGroup)
        {
            blockRenderer = GetComponent<Renderer>();
            if (blockRenderer != null)
            {
                originalColor = blockRenderer.material.color;
            }
            else
            {
                Debug.LogError($"Renderer is missing on {gameObject.name}");
            }
        }
        originalScale = transform.localScale;
    }

    // OnMouseDown for selection
    void OnMouseDown()
    {
        if (SelectableBlock.objectManager == null) // Ensure ObjectManager is initialized
        {
            Debug.LogError("ObjectManager is null. Please ensure it is assigned.");
            return;
        }
    
        // Handle block selection/deselection from ObjectManager
        SelectableBlock.objectManager.HandleBlockClick(this);

        // Sidebar logic
        SidebarManager.Instance?.UpdateSidebar(this);
    }

    // Highlighting logic
    public void Highlight()
    {
        if (isGroup)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SelectableBlock>()?.Highlight();
            }
        }
        else if (blockRenderer != null)
        {
            // Apply highlight color unless a custom color is already applied
            if (blockRenderer.material.color == originalColor)
            {
                blockRenderer.material.color = selectedColor;
            }
        }
    }

    public void Deselect()
    {
        if (isGroup)
        {
            // Deselect all child blocks in the group
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<SelectableBlock>()?.Deselect();
            }
        }
        else if (blockRenderer != null) 
        {
            // Reset to original color only if not set to a custom color
            if (blockRenderer.material.color == selectedColor) // Only revert selected blocks
            {
                blockRenderer.material.color = originalColor;
            }
        }
    }

    // Accessors
    public string GetName() => gameObject.name;
    public Vector3 GetPosition() => transform.position;
    public void SetPosition(Vector3 newPosition) => transform.position = newPosition;
    public Vector3 GetRotation() => transform.eulerAngles;
    public void SetRotation(Vector3 newRotation) => transform.rotation = Quaternion.Euler(newRotation);
    public Vector3 GetScale() => transform.localScale;

    // Set scale using scalable flags
    public void SetScale(Vector3 newScale)
    {
        newScale.x = (scalableX ? Mathf.Max(1, newScale.x) : 1) * originalScale.x;
        newScale.y = (scalableY ? Mathf.Max(1, newScale.y) : 1) * originalScale.y;
        newScale.z = (scalableZ ? Mathf.Max(1, newScale.z) : 1) * originalScale.z;

        transform.localScale = newScale;
    }

    // Apply color to the block
    public void ApplyColor(Color color)
    {
        if (blockRenderer != null)
        {
            blockRenderer.material.color = color;
            isCustomColor = true; // Mark this block as using a custom color
        }
    }

    public bool IsGroup() => isGroup;

    public bool[] GetScalable() => new bool[] { scalableX, scalableY, scalableZ };

    // Set object manager
    public static void SetObjectManager(ObjectManager om)
    {
        objectManager = om;
    }
}