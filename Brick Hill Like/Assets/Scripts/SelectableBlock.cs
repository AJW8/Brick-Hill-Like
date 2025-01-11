using UnityEngine;
public class SelectableBlock : MonoBehaviour
{
	[SerializeField] private Color selectedColor;
	[SerializeField] private bool scalableX, scalableY, scalableZ, isGroup;

    private Renderer blockRenderer;
    private Color originalColor;
    public static SelectableBlock SelectedBlock;
	public static ObjectManager ObjectManager;

	private Vector3 originalScale;

    void Start()
    {
		if (!isGroup)
		{
			blockRenderer = GetComponent<Renderer> ();
			originalColor = blockRenderer.material.color;
		}
		originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
		if (SelectedBlock != null)
        {
			ObjectManager.DeselectBlock (this);
            SelectedBlock.Deselect();
			return;
        }
		ObjectManager.SelectBlock (this);
        SelectedBlock = this;
        Highlight();
        SidebarManager.Instance.UpdateSidebar(this);
    }

	public void Highlight()
    {
		if (isGroup)
		{
			foreach (Transform child in transform) child.gameObject.GetComponent<SelectableBlock> ().Highlight ();
		}
		else blockRenderer.material.color = selectedColor;
    }

    public void Deselect()
    {
		if (isGroup)
		{
			foreach (Transform child in transform) child.gameObject.GetComponent<SelectableBlock> ().Deselect ();
		}
        else blockRenderer.material.color = originalColor;
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
        // Ensure the scale is valid (non-zero and positive integer and block is scalable in that dimension)
		newScale.x = (scalableX ? (int)Mathf.Max(1, newScale.x) : 1) * originalScale.x;
		newScale.y = (scalableY ? (int)Mathf.Max(1, newScale.y) : 1) * originalScale.y;
		newScale.z = (scalableZ ? (int)Mathf.Max(1, newScale.z) : 1) * originalScale.z;

        transform.localScale = newScale;
    }
    
    public void ApplyColor(Color newColor)
    {
        blockRenderer.material.color = newColor;
        originalColor = newColor; // Update the original color to persist the change
    }

	public bool[] GetScalable()
	{
		return new bool[]{ scalableX, scalableY, scalableZ };
	}

	public bool IsGroup()
	{
		return isGroup;
	}

	public static void SetObjectManager(ObjectManager om)
	{
		ObjectManager = om;
	}
}
