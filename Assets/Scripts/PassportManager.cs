using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassportManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passportText; // Passport details text
    public Image applicantImage;         // Applicant's face image
    public Image regionSymbol;           // Region symbol

    [Header("UI Panels")]
    public GameObject applicantPanel;    // Panel with applicant's image
    public GameObject documentPanel;     // Panel for passport details

    [Header("Decision Buttons")]
    public Button approveButton;         // Approve button
    public Button denyButton;            // Deny button

    [Header("Passport Settings")]
    private int currentYear = 4065;      // Base year in the game
    private int minValidYears = 1;       // Minimum years for validity
    private int maxValidYears = 5;       // Maximum years for validity
    private int expiredYearsRange = 3;   // How far back an expired passport can go

    private string[] alienNames = { "Zarqa Elion", "Nebulo Xel", "Quorin Arak", "Vetra Shiran", "Xilra Talos" };
    private string[] alienOrigins = { "Andromeda Prime", "Galva-Theta", "Nebulon IV", "Xyron-9", "Zarquinia" };

    private Sprite[] alienImages;
    private Sprite[] regionSymbols;

    private string currentName;
    private string currentOrigin;
    private int birthYear;
    private int expirationYear;
    private Sprite currentFaceImage;
    private Sprite currentRegionSymbol;

    private bool isResourcesLoaded = false;

    private void Awake()
    {
        LoadResources();
    }

    public void LoadResources()
    {
        if (isResourcesLoaded) return;

        // Load assets from Resources
        alienImages = Resources.LoadAll<Sprite>("Images/Aliens");
        regionSymbols = Resources.LoadAll<Sprite>("Images/RegionSymbols");

        if (alienImages.Length > 0 && regionSymbols.Length > 0)
        {
            isResourcesLoaded = true;
            Debug.Log("Resources loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load resources. Check file paths.");
        }
    }

    public bool AreResourcesLoaded()
    {
        return isResourcesLoaded;
    }

    public void GeneratePassport()
    {
        if (!isResourcesLoaded)
        {
            Debug.LogError("Resources not loaded!");
            return;
        }

        int randomIndex = Random.Range(0, alienNames.Length);
        currentName = alienNames[randomIndex];
        currentOrigin = alienOrigins[randomIndex];
        currentFaceImage = alienImages[Random.Range(0, alienImages.Length)];
        currentRegionSymbol = regionSymbols[randomIndex];

        // Generate birth year and expiration year
        GenerateRandomDates();

        UpdateApplicantPanel();
    }

    private void GenerateRandomDates()
    {
        // Birth year: Random between currentYear - 50 and currentYear - 18
        birthYear = Random.Range(currentYear - 50, currentYear - 18);

        // Expiration year: 80% chance valid, 20% chance expired
        if (Random.value < 0.8f)
        {
            expirationYear = currentYear + Random.Range(minValidYears, maxValidYears + 1); // Future date
        }
        else
        {
            expirationYear = currentYear - Random.Range(1, expiredYearsRange + 1); // Past date (expired)
        }
    }

    private void UpdateApplicantPanel()
    {
        applicantImage.sprite = currentFaceImage;
        regionSymbol.sprite = currentRegionSymbol;
        applicantPanel.SetActive(true);
    }

    public void OpenDocument()
    {
        documentPanel.SetActive(true);
        passportText.text = $"Name: {currentName}\n" +
                            $"Date of Birth: {birthYear}\n" +
                            $"Origin: {currentOrigin}\n" +
                            $"Expiration Year: {expirationYear}";
        EnableDecisionButtons(false);
    }

    public void CloseDocument()
    {
        documentPanel.SetActive(false);
        EnableDecisionButtons(true);
    }

    private void EnableDecisionButtons(bool enable)
    {
        if (approveButton != null) approveButton.interactable = enable;
        if (denyButton != null) denyButton.interactable = enable;
    }
}
