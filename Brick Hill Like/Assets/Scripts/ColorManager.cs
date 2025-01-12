using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // Sets the color of the currently selected block
    public void SetColor(string colorName)
    {
        // Check if a block is currently selected
        if (SelectableBlock.SelectedBlock != null)
        {
            Color selectedColor;

            // Try to parse the given string into a color
            if (ColorUtility.TryParseHtmlString(colorName, out selectedColor))
            {
                // Apply the parsed color to the selected block
                SelectableBlock.SelectedBlock.ApplyColor(selectedColor);
            }
        }
    }
}