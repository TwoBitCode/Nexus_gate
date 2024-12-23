using UnityEngine;

public class RulesPanelManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rulesPanel; // Reference to the rules panel

    public void ToggleRulesPanel(bool isOpen)
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(isOpen);
            Debug.Log($"Rules panel {(isOpen ? "opened" : "closed")}.");
        }
        else
        {
            Debug.LogError("Rules panel reference is missing in the inspector!");
        }
    }
}
