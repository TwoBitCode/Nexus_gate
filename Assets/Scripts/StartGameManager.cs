using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;  
    public GameObject rulesPanel;  
    public GameObject gameUI;  

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start button reference is missing in the inspector!");
        }
    }

    private void StartGame()
    {
        Debug.Log("StartGame function called!");

        // Close the rules panel if active
        if (rulesPanel != null && rulesPanel.activeSelf)
        {
            rulesPanel.SetActive(false);
            Debug.Log("Rules panel closed.");
        }
        else
        {
            Debug.LogWarning("Rules panel was already closed or missing.");
        }

        // Enable the game UI
        if (gameUI != null)
        {
            gameUI.SetActive(true);
            Debug.Log("Game UI enabled.");
        }
        else
        {
            Debug.LogWarning("Game UI reference is missing!");
        }
    }
}
