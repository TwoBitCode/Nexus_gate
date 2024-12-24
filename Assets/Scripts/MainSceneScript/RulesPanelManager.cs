using TMPro;
using UnityEngine;

public class RulesPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rulesPanel;
    public TextMeshProUGUI rulesText;

    public void ShowRules(DayData dayData)
    {
        if (rulesPanel != null && rulesText != null)
        {
            rulesPanel.SetActive(true);
            rulesText.text = string.Join("\n", dayData.newRules);
        }
        else
        {
            Debug.LogError("Rules panel or text reference is missing.");
        }
    }

    public void HideRules()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }
    }
}
