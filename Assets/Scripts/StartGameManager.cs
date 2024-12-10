using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    public Button startButton; // Reference to the Start Button

    void Start()
    {
        // Attach a listener to the button
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        // Load the main game scene or activate the game
        Debug.Log("Game Started!");

        // If you want to close the rules panel:
        gameObject.SetActive(false);
    }
}
