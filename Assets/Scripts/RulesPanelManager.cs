using UnityEngine;

public class RulesPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rulesPanel; // Reference to the rules panel

    public void OpenRulesPanel()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(true);
            Debug.Log("Rules panel opened.");
        }
        else
        {
            Debug.LogError("Rules panel reference is missing in the inspector!");
        }
    }

    public void CloseRulesPanel()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
            Debug.Log("Rules panel closed.");
        }
        else
        {
            Debug.LogError("Rules panel reference is missing in the inspector!");
        }
    }
}
