using UnityEngine;
using UnityEngine.UI;

public class SidebarManager : MonoBehaviour
{
    public static SidebarManager Instance;

    public Text objectNameText;
    public InputField positionXInput, positionYInput, positionZInput;
    public InputField rotationXInput, rotationYInput, rotationZInput;
    public InputField scaleXInput, scaleYInput, scaleZInput;

	[SerializeField] GameObject scaleXPlaceholder, scaleYPlaceholder, scaleZPlaceholder;

    private SelectableBlock currentBlock;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateSidebar(SelectableBlock block)
    {
		bool[] scalable = block.GetScalable ();
		scaleXPlaceholder.SetActive(!scalable [0]);
		scaleYPlaceholder.SetActive(!scalable [1]);
		scaleZPlaceholder.SetActive(!scalable [2]);

        currentBlock = block;

        objectNameText.text = block.GetName();

        Vector3 position = block.GetPosition();
        positionXInput.text = position.x.ToString();
        positionYInput.text = position.y.ToString();
        positionZInput.text = position.z.ToString();

        Vector3 rotation = block.GetRotation();
        rotationXInput.text = rotation.x.ToString();
        rotationYInput.text = rotation.y.ToString();
        rotationZInput.text = rotation.z.ToString();

        Vector3 scale = block.GetScale();
		scaleXInput.text = ((int)scale.x).ToString();
		scaleYInput.text = ((int)scale.y).ToString();
		scaleZInput.text = ((int)scale.z).ToString();
    }

    public void OnPositionChanged()
    {
        if (currentBlock == null) return;

        Vector3 newPosition = new Vector3(
            float.Parse(positionXInput.text),
            float.Parse(positionYInput.text),
            float.Parse(positionZInput.text)
        );
        currentBlock.SetPosition(newPosition);
    }

    public void OnRotationChanged()
    {
        if (currentBlock == null) return;

        Vector3 newRotation = new Vector3(
            float.Parse(rotationXInput.text),
            float.Parse(rotationYInput.text),
            float.Parse(rotationZInput.text)
        );
        currentBlock.SetRotation(newRotation);
    }

    public void OnScaleChanged()
    {
        if (currentBlock == null) return;

        // Declare variables beforehand
        float x, y, z;

		//Check which dimensions are scalable for this object
		bool[] scalable = currentBlock.GetScalable();

		// Parse scale values and ensure they are valid (positive integers)
		int scaleX = scalable[0] ? (int)Mathf.Max(1, float.TryParse(scaleXInput.text, out x) ? x : currentBlock.GetScale().x) : 1;
		int scaleY = scalable[1] ? (int)Mathf.Max(1, float.TryParse(scaleYInput.text, out y) ? y : currentBlock.GetScale().y) : 1;
		int scaleZ = scalable[2] ? (int)Mathf.Max(1, float.TryParse(scaleZInput.text, out z) ? z : currentBlock.GetScale().z) : 1;

        Vector3 newScale = new Vector3(scaleX, scaleY, scaleZ);
        currentBlock.SetScale(newScale);
    }

}
