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
    [SerializeField] private int maxReputation = 100; // Maximum reputation value, customizable in the Inspector
    public AudioSource audioSource; // Reference to the AudioSource in the scene

    [Header("Game Managers")]
    public ApplicantManager applicantManager; // Reference to the ApplicantManager
    public DayManager dayManager; // Reference to the DayManager
    private UIManagerMainScene uiManager; // Reference to the centralized UIManager

    private int currentReputation; // Current reputation tracker

    private void Start()
    {
        // Get UIManager component and validate manager assignments
        uiManager = GetComponent<UIManagerMainScene>();
        if (dayManager == null || uiManager == null || applicantManager == null)
        {
            Debug.LogError("DayManager, ApplicantManager, or UIManager is missing on StartGameManager.");
            return;
        }

        // Initialize reputation and update the UI
        currentReputation = maxReputation;
        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);

        // Initialize the day and set up UI elements
        InitializeDay();
    }

    private void InitializeDay()
    {
        if (dayManager == null)
        {
            Debug.LogError("DayManager is not assigned!");
            return;
        }

        // Initialize the current day
        dayManager.InitializeDay();

        // Get the current day's data and set up the UI accordingly
        DayData currentDayData = dayManager.GetCurrentDayData();
        if (currentDayData == null) return;

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

    public UIApplicantPanelManager applicantPanelManager; // Reference to the correct UI manager

    private IEnumerator StartGameRoutine()
    {
        DayData currentDayData = dayManager.GetCurrentDayData();
        if (currentDayData == null)
        {
            Debug.LogError("DayData is missing or not assigned in DayManager!");
            yield break;
        }

        // Log the maxApplicantsPerDay for verification
        Debug.Log($"DayData maxApplicantsPerDay: {currentDayData.maxApplicantsPerDay}");

        // Set up applicants for the day
        applicantManager.SetDayData(currentDayData);

        // Generate and validate the first applicant
        if (!applicantManager.GenerateNextApplicant())
        {
            Debug.LogError("Failed to generate the first applicant.");
            yield break;
        }

        Applicant firstApplicant = applicantManager.GetCurrentApplicant();
        if (firstApplicant == null)
        {
            Debug.LogError("First applicant generation failed!");
            yield break;
        }

        // Update the UI for the game start
        uiManager.HidePanel(rulesPanel);
        uiManager.ShowPanel(applicantPanel);
        applicantPanelManager.UpdateApplicantUI(firstApplicant);

        Debug.Log("Game started successfully.");
        yield break;
    }

    public void PlayDailyMusic(DayData dayData)
    {
        if (audioSource == null || dayData == null || dayData.dailyMusic == null)
        {
            Debug.LogError("AudioSource or dailyMusic is not assigned!");
            return;
        }

        // Play the daily music with looping enabled
        audioSource.clip = dayData.dailyMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
