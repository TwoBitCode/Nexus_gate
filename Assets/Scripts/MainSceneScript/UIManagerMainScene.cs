using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMainScene : MonoBehaviour
{
    public void ShowPanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void UpdateText(TextMeshProUGUI textElement, string message)
    {
        if (textElement != null)
            textElement.text = message;
    }

    public void DisableAllButtons()
    {
        Button[] allButtons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }
}
