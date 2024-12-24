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
    public AudioSource audioSource; // Reference to the AudioSource in the scene

    [Header("Game Managers")]
    public ApplicantManager applicantManager; // Reference to the ApplicantManager
    public DayManager dayManager; // Reference to the DayManager
    private UIManagerMainScene uiManager; // Reference to the centralized UIManager
    private int currentReputation;
    private void Start()
    {
        uiManager = GetComponent<UIManagerMainScene>();
        if (dayManager == null || uiManager == null || applicantManager == null)
        {
            Debug.LogError("DayManager, ApplicantManager, or UIManager is missing on StartGameManager.");
            return;
        }

        currentReputation = maxReputation; // Initialize reputation
        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation); // Update UI

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

        // Set up the UI for the current day
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

        applicantManager.SetDayData(currentDayData);

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

        audioSource.clip = dayData.dailyMusic;
        audioSource.loop = true; // Ensure the music loops
        audioSource.Play();
    }
}
