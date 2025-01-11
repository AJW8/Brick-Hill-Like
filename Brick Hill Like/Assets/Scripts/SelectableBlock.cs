using UnityEngine;
public class SelectableBlock : MonoBehaviour
{
    private Renderer blockRenderer;
    private Color originalColor;
    public static SelectableBlock SelectedBlock;

    void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        originalColor = blockRenderer.material.color;
    }

    void OnMouseDown()
    {
        if (SelectedBlock != null)
        {
            SelectedBlock.Deselect();
        }

        SelectedBlock = this;
        Highlight();
        SidebarManager.Instance.UpdateSidebar(this);
    }

    public void Highlight()
    {
        blockRenderer.material.color = Color.yellow; // Highlight color
    }

    public void Deselect()
    {
        blockRenderer.material.color = originalColor;
    }

    public string GetName()
    {
        return gameObject.name;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetRotation()
    {
        return transform.eulerAngles;
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetRotation(Vector3 newRotation)
    {
        transform.eulerAngles = newRotation;
    }

    public void SetScale(Vector3 newScale)
    {
        // Ensure the scale is valid (non-zero and positive)
        newScale.x = Mathf.Max(0.01f, newScale.x);
        newScale.y = Mathf.Max(0.01f, newScale.y);
        newScale.z = Mathf.Max(0.01f, newScale.z);

        transform.localScale = newScale;
    }
    
    public void ApplyColor(Color newColor)
    {
        blockRenderer.material.color = newColor;
        originalColor = newColor; // Update the original color to persist the change
    }
}
