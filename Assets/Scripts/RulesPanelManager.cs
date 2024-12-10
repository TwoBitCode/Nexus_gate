using UnityEngine;
using UnityEngine.UI;

public class RulesPanelManager : MonoBehaviour
{
    public GameObject rulesPanel; // Reference to the Rules Panel
    public Button startButton; // Reference to the Start Button

    void Start()
    {
        // Ensure the panel is active when the game starts
        rulesPanel.SetActive(true);

        // Attach a listener to the button
        startButton.onClick.AddListener(CloseRulesPanel);
    }

    void CloseRulesPanel()
    {
        // Hide the rules panel
        rulesPanel.SetActive(false);
    }
}
