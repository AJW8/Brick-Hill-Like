using UnityEngine;
using UnityEngine.UI;

public class SidebarManager : MonoBehaviour
{
    public static SidebarManager Instance;

    public Text objectNameText;
    public InputField positionXInput, positionYInput, positionZInput;
    public InputField rotationXInput, rotationYInput, rotationZInput;
    public InputField scaleXInput, scaleYInput, scaleZInput;

    private SelectableBlock currentBlock;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateSidebar(SelectableBlock block)
    {
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
        scaleXInput.text = scale.x.ToString();
        scaleYInput.text = scale.y.ToString();
        scaleZInput.text = scale.z.ToString();
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

        // Parse scale values and ensure they are valid
        float scaleX = Mathf.Max(0.01f, float.TryParse(scaleXInput.text, out x) ? x : currentBlock.GetScale().x);
        float scaleY = Mathf.Max(0.01f, float.TryParse(scaleYInput.text, out y) ? y : currentBlock.GetScale().y);
        float scaleZ = Mathf.Max(0.01f, float.TryParse(scaleZInput.text, out z) ? z : currentBlock.GetScale().z);

        Vector3 newScale = new Vector3(scaleX, scaleY, scaleZ);
        currentBlock.SetScale(newScale);
    }

}
