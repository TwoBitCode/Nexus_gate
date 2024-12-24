using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StartGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public GameObject rulesPanel;
    public TextMeshProUGUI rulesText;
    public GameObject applicantPanel;
    public GameObject documentPanel;
    public Slider reputationBar;
    public int maxReputation = 100;

    [Header("Game Managers")]
    public PassportManager passportManager;
    public DayManager dayManager; // Reference to the DayManager
    private UIManagerMainScene uiManager; // Reference to the centralized UIManager

    private void Start()
    {
        uiManager = GetComponent<UIManagerMainScene>();
        if (dayManager == null || uiManager == null)
        {
            Debug.LogError("DayManager or UIManager is missing on StartGameManager.");
            return;
        }

        InitializeDay();
    }

    private void InitializeDay()
    {
        DayData currentDayData = dayManager.GetCurrentDayData();
        if (currentDayData == null)
        {
            Debug.LogError("DayData is missing for the current day!");
            return;
        }

        // Set up the UI for the current day
        uiManager.ShowPanel(rulesPanel);
        uiManager.UpdateText(rulesText, string.Join("\n", currentDayData.newRules));
        uiManager.HidePanel(applicantPanel);
        uiManager.HidePanel(documentPanel);

        // Initialize the reputation bar
        if (reputationBar != null)
        {
            reputationBar.maxValue = maxReputation;
            reputationBar.value = maxReputation;
        }

        // Set up the start button
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        DayData currentDayData = dayManager.GetCurrentDayData();
        if (passportManager == null || currentDayData == null)
        {
            Debug.LogError("PassportManager or DayData is missing!");
            yield break;
        }

        // Pass the DayData to PassportManager
        passportManager.SetDayData(currentDayData);

        // Transition to applicant panel
        uiManager.HidePanel(rulesPanel);
        uiManager.ShowPanel(applicantPanel);

        // Generate the first passport
        passportManager.GeneratePassport();
        Debug.Log("Game started: Passport generated successfully.");

        yield break;
    }
}
