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

    [Header("Age Settings")]
    [SerializeField] private int minAlienAge = 25;
    [SerializeField] private int maxAlienAge = 120;
    [SerializeField] private int minFakeAge = 5;
    [SerializeField] private int maxFakeAge = 150;

    private string[] alienNames = { "Zarqa Elion", "Nebulo Xel", "Quorin Arak", "Vetra Shiran", "Xilra Talos" };
    private string[] alienOrigins = { "Andromeda Prime", "Galva-Theta", "Nebulon IV", "Xyron-9", "Zarquinia" };

    private string[] fakeNames = { "Zevon-X", "Aria-12", "Rho Delta", "Liran-77", "Korin Prime" };
    private string[] fakeOrigins = { "Void Nexus", "Aether Zone", "Lost Sector-9", "Phantom Ring", "Oblivion Expanse" };

    private Sprite[] alienImages;
    private Sprite[] regionSymbols;
    private Sprite[] fakeRegionSymbols;

    private string currentName;
    private string currentOrigin;
    private int currentAge;
    private Sprite currentFaceImage;
    private Sprite currentRegionSymbol;

    private bool isResourcesLoaded = false;
    public int MinAlienAge { get { return minAlienAge; } }
    public int MaxAlienAge { get { return maxAlienAge; } }

    private void Awake()
    {
        LoadResources();
        Debug.Log("PassportManager Awake: Resources loaded successfully.");
    }

    public bool AreResourcesLoaded()
    {
        return isResourcesLoaded;
    }


    public void LoadResources()
    {
        if (isResourcesLoaded) return; // Avoid reloading resources

        // Load all resources
        alienImages = Resources.LoadAll<Sprite>("Images/Aliens");
        regionSymbols = Resources.LoadAll<Sprite>("Images/RegionSymbols");
        fakeRegionSymbols = Resources.LoadAll<Sprite>("Images/FakeRegionSymbols");

        if (alienImages.Length == 0 || regionSymbols.Length == 0 || fakeRegionSymbols.Length == 0)
        {
            Debug.LogError("Resources failed to load! Check the folder paths.");
        }
        else
        {
            isResourcesLoaded = true;
            Debug.Log("Resources loaded successfully.");
        }
    }


    public bool IsResourcesLoaded()
    {
        return isResourcesLoaded;
    }

    public void GeneratePassport()
    {
        if (!isResourcesLoaded)
        {
            Debug.LogError("Resources are still not loaded! Cannot generate passport.");
            return;
        }

        Debug.Log("Generating Passport...");
        float randomType = Random.value;

        if (randomType < 0.8f) // Valid applicant
        {
            int index = Random.Range(0, alienNames.Length);
            currentName = alienNames[index];
            currentOrigin = alienOrigins[index];
            currentAge = Random.Range(minAlienAge, maxAlienAge);
            currentFaceImage = alienImages[Random.Range(0, alienImages.Length)];
            currentRegionSymbol = regionSymbols[index];
        }
        else // Fake applicant
        {
            int index = Random.Range(0, fakeNames.Length);
            currentName = fakeNames[index];
            currentOrigin = fakeOrigins[index];
            currentAge = Random.Range(minFakeAge, maxFakeAge);
            currentFaceImage = alienImages[Random.Range(0, alienImages.Length)];
            currentRegionSymbol = fakeRegionSymbols[index];
        }

        UpdateApplicantPanel();
    }

    private void UpdateApplicantPanel()
    {
        applicantImage.sprite = currentFaceImage;
        applicantPanel.SetActive(true);
        Debug.Log("Applicant updated on the panel.");
    }

    public void OpenDocument()
    {
        documentPanel.SetActive(true);
        passportText.text = $"Name: {currentName}\nAge: {currentAge}\nOrigin: {currentOrigin}";
        regionSymbol.sprite = currentRegionSymbol;

        EnableDecisionButtons(false); // Disable buttons while viewing document
    }

    public void CloseDocument()
    {
        documentPanel.SetActive(false);
        EnableDecisionButtons(true); // Re-enable buttons
    }

    private void EnableDecisionButtons(bool enable)
    {
        if (approveButton != null) approveButton.interactable = enable;
        if (denyButton != null) denyButton.interactable = enable;
    }
}
