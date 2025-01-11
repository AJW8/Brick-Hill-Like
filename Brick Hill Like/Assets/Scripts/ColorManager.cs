using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public void SetColor(string colorName)
    {
        if (SelectableBlock.SelectedBlock != null)
        {
            Color selectedColor;
            if (ColorUtility.TryParseHtmlString(colorName, out selectedColor))
            {
                SelectableBlock.SelectedBlock.ApplyColor(selectedColor);
            }
        }
    }
}
