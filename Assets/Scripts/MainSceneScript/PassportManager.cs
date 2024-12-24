using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PassportManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passportText;
    public Image applicantImage;
    public Image regionSymbol;

    [Header("UI Panels")]
    public GameObject applicantPanel;
    public GameObject documentPanel;

    [Header("Decision Buttons")]
    public Button approveButton;
    public Button denyButton;

    [Header("Random Applicant Data")]
    public Sprite[] alienImages;
    public Sprite[] regionSymbols;
    public string[] alienNames = { "Zarqa Elion", "Nebulo Xel", "Quorin Arak", "Vetra Shiran", "Xilra Talos" };

    [Header("Applicant Handling")]
    private DayData currentDayData;
    public int maxApplicantsPerDay = 5;
    private int currentApplicantIndex = 0;

    private string currentName;
    private string currentOrigin;
    private int birthYear;
    private int expirationYear;
    private Sprite currentFaceImage;
    private Sprite currentRegionSymbol;

    private List<OriginSymbolPair> shuffledPairs;
    [Header("End of Day Settings")]
    public GameObject gameOverPanel; // Reference to the GameOverPanel


    public void SetDayData(DayData dayData)
    {
        if (dayData == null)
        {
            Debug.LogError("SetDayData received a null DayData!");
            return;
        }

        currentDayData = dayData;

        if (currentDayData.originSymbolPairs == null || currentDayData.originSymbolPairs.Length == 0)
        {
            Debug.LogError("originSymbolPairs in currentDayData is null or empty!");
            return;
        }

        shuffledPairs = new List<OriginSymbolPair>(currentDayData.originSymbolPairs);
        shuffledPairs = shuffledPairs.OrderBy(a => Random.value).ToList();

        Debug.Log("Applicant origins and symbols shuffled successfully.");
    }

    public void GeneratePassport()
    {
        Debug.Log($"GeneratePassport called for applicant {currentApplicantIndex}.");

        if (currentApplicantIndex >= maxApplicantsPerDay)
        {
            Debug.Log("No more applicants for today.");
            EndDay();
            return;
        }

        if (shuffledPairs == null || shuffledPairs.Count == 0)
        {
            Debug.LogError("ShuffledPairs is null or empty.");
            return;
        }

        // Use shuffled pairs to get origin and symbol
        OriginSymbolPair pair = shuffledPairs[currentApplicantIndex % shuffledPairs.Count];
        currentOrigin = pair.origin;
        currentRegionSymbol = pair.symbol;

        currentName = alienNames[Random.Range(0, alienNames.Length)];
        currentFaceImage = alienImages[Random.Range(0, alienImages.Length)];

        // Decide if the applicant is invalid
        bool isInvalidApplicant = Random.value < 0.3f; // 30% chance of being invalid

        if (isInvalidApplicant)
        {
            GenerateInvalidData(); // Generate invalid applicant data
        }
        else
        {
            GenerateRandomDates(); // Generate valid data
        }

        UpdateApplicantPanel();
    }

    private void GenerateInvalidData()
    {
        int invalidType = Random.Range(0, 3); // 0: Invalid Birth Year, 1: Expired Passport, 2: Mismatched Symbol

        switch (invalidType)
        {
            case 0: // Invalid Birth Year
                birthYear = Random.Range(6080, 7000); // Unrealistic year
                Debug.Log("Invalid applicant: Unrealistic birth year.");
                break;

            case 1: // Expired Passport
                int currentYear = 4065;
                expirationYear = Random.Range(currentYear - 10, currentYear - 1); // Expired year
                Debug.Log("Invalid applicant: Expired passport.");
                break;

            case 2: // Mismatched Origin-Symbol Pair
                currentRegionSymbol = regionSymbols[Random.Range(0, regionSymbols.Length)]; // Random symbol
                Debug.Log("Invalid applicant: Origin and symbol mismatch.");
                break;
        }
    }



    private void GenerateRandomDates()
    {
        int currentYear = 4065;
        birthYear = Random.Range(currentYear - 50, currentYear - 18); // Logical birth year
        expirationYear = Random.value < 0.8f ?
            currentYear + Random.Range(1, 6) :  // Valid expiration
            currentYear - Random.Range(1, 4);  // Expired passport (valid passports will override this)
    }

    private void GenerateMismatchedOriginSymbol()
    {
        // Keep the current origin but assign a random incorrect symbol
        List<Sprite> allSymbols = new List<Sprite>(regionSymbols);
        allSymbols.Remove(currentRegionSymbol); // Remove the valid symbol

        if (allSymbols.Count > 0)
        {
            currentRegionSymbol = allSymbols[Random.Range(0, allSymbols.Count)];
        }
    }


    private void CheckApplicantValidity()
    {
        bool isBirthYearValid = birthYear <= 4065 && birthYear >= 4015;
        bool isExpirationValid = expirationYear >= 4065;
        bool isSymbolValid = shuffledPairs.Any(pair => pair.origin == currentOrigin && pair.symbol == currentRegionSymbol);

        if (!isBirthYearValid || !isExpirationValid || !isSymbolValid)
        {
            Debug.Log($"Invalid applicant: BirthYearValid={isBirthYearValid}, ExpirationValid={isExpirationValid}, SymbolValid={isSymbolValid}");
        }
    }

    private void UpdateApplicantPanel()
    {
        if (applicantImage == null || regionSymbol == null || passportText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the PassportManager.");
            return;
        }

        applicantImage.sprite = currentFaceImage;
        regionSymbol.sprite = currentRegionSymbol;
        passportText.text = $"Name: {currentName}\n" +
                            $"Date of Birth: {birthYear}\n" +
                            $"Origin: {currentOrigin}\n" +
                            $"Expiration Year: {expirationYear}";

        applicantPanel.SetActive(true);
    }

    public bool GenerateNextPassport()
    {
        if (currentApplicantIndex >= maxApplicantsPerDay)
        {
            Debug.Log("No more applicants for today.");
            EndDay();
            return false;
        }

        currentApplicantIndex++;
        GeneratePassport();
        return true;
    }
    [Header("Game Controller")]
    public GameController gameController;

    private void EndDay()
    {
        Debug.Log("End of Day! All applicants have been processed.");
        applicantPanel.SetActive(false);
        documentPanel.SetActive(false);

        if (gameController != null)
        {
            gameController.ShowEndOfDayMessage(false); // Day complete message
        }
        else
        {
            Debug.LogError("GameController is not assigned in PassportManager!");
        }
    }


    public void OpenDocument()
    {
        Debug.Log("OpenDocument called: Showing passport details.");
        if (documentPanel != null)
        {
            documentPanel.SetActive(true); // Show the document panel
        }

        passportText.text = $"Name: {currentName}\n" +
                            $"Date of Birth: {birthYear}\n" +
                            $"Origin: {currentOrigin}\n" +
                            $"Expiration Year: {expirationYear}";

        EnableDecisionButtons(false);
    }

    public void CloseDocument()
    {
        Debug.Log("CloseDocument called: Hiding passport details.");
        if (documentPanel != null)
        {
            documentPanel.SetActive(false); // Hide the document panel
        }

        EnableDecisionButtons(true);
    }

    private void EnableDecisionButtons(bool enable)
    {
        if (approveButton != null) approveButton.interactable = enable;
        if (denyButton != null) denyButton.interactable = enable;

        Debug.Log($"Decision buttons interactable: {enable}");
    }
    public bool IsValidOriginSymbol(string origin, Sprite symbol)
    {
        return shuffledPairs.Any(pair => pair.origin == origin && pair.symbol == symbol);
    }

}
