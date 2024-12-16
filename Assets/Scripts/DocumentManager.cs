using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassportManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passportText; // Displays applicant's passport details
    public Image applicantImage; // Displays applicant's face
    public Image regionSymbol; // Displays the symbol for the applicant's origin
    private Sprite[] fakeSymbols; // Fake region symbols

    private string[] alienNames = { "Zarqa Elion", "Nebulo Xel", "Quorin Arak", "Vetra Shiran", "Xilra Talos" };
    private string[] alienOrigins = { "Andromeda Prime", "Galva-Theta", "Nebulon IV", "Xyron-9", "Zarquinia" };
    private Sprite[] alienSymbols; // Symbols for the alien origins

    private string[] fakeNames = { "Zevon-X", "Aria-12", "Rho Delta", "Liran-77", "Korin Prime" };
    private string[] fakeOrigins = { "Void Nexus", "Aether Zone", "Lost Sector-9", "Phantom Ring", "Oblivion Expanse" };

    private Sprite[] alienImages;

    [Header("Applicant Settings")]
    public int minAlienAge = 25;
    public int maxAlienAge = 120;
    public int minFakeAge = 5;
    public int maxFakeAge = 150;

    private void Start()
    {
        // Load images from the Resources folder
        alienImages = LoadImagesFromResources("Images/Aliens");
        alienSymbols = LoadImagesFromResources("Images/RegionSymbols");
        Debug.Log($"Loaded {alienImages?.Length ?? 0} alien images and {alienSymbols?.Length ?? 0} region symbols.");
        fakeSymbols = LoadImagesFromResources("Images/FakeRegionSymbols");

        if (fakeSymbols.Length != fakeOrigins.Length)
        {
            Debug.LogError("Fake symbols count does not match fake origins count!");
        }

        // Verify loaded images
        if (alienImages.Length == 0 || alienSymbols.Length == 0)
        {
            Debug.LogError("Failed to load images. Make sure images exist in Resources/Images.");
            return;
        }

        // Generate the first passport
        GeneratePassport();
    }

    private Sprite[] LoadImagesFromResources(string resourceFolder)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(resourceFolder);

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"No images found in folder: Resources/{resourceFolder}. Check folder structure and sprite settings.");
            return new Sprite[0]; // Return an empty array to avoid null reference
        }

        Debug.Log($"Successfully loaded {sprites.Length} images from Resources/{resourceFolder}");
        return sprites;
    }

    public void GeneratePassport()
    {
        if (passportText == null || applicantImage == null || regionSymbol == null)
        {
            Debug.LogError("Missing UI elements. Please check PassportManager references.");
            return;
        }

        string name, origin;
        Sprite faceImage = null;
        Sprite symbolImage = null;
        int age;

        float randomType = Random.value;

        if (randomType < 0.8f && alienImages != null && alienImages.Length > 0)
        {
            // Generate a valid alien applicant
            int originIndex = Random.Range(0, alienOrigins.Length);
            name = alienNames[originIndex % alienNames.Length]; // Match name order to origins
            origin = alienOrigins[originIndex];
            faceImage = alienImages[Random.Range(0, alienImages.Length)];
            symbolImage = alienSymbols[originIndex]; // Match symbol to origin order
            age = Random.Range(minAlienAge - 5, maxAlienAge + 5);
        }
        else
        {
            // Generate a fake applicant with a random image
            int fakeIndex = Random.Range(0, fakeOrigins.Length);
            name = fakeNames[fakeIndex % fakeNames.Length];
            origin = fakeOrigins[fakeIndex];

            // Assign the fake symbol
            symbolImage = fakeSymbols[fakeIndex];

            // Use random alien images for fake applicants
            if (alienImages != null && alienImages.Length > 0)
            {
                faceImage = alienImages[Random.Range(0, alienImages.Length)];
            }
            else
            {
                Debug.LogWarning("No alien images available, loading default image.");
                faceImage = Resources.Load<Sprite>("Images/DefaultImage");
            }

            age = Random.Range(minFakeAge, maxFakeAge);
        }

        // Update passport details text
        passportText.text = $"Name: {name}\nAge: {age}\nOrigin: {origin}";

        // Update applicant image
        if (faceImage != null)
        {
            applicantImage.sprite = faceImage;
        }
        else
        {
            Debug.LogWarning("No image found, using default placeholder.");
            applicantImage.sprite = Resources.Load<Sprite>("Images/DefaultImage");
        }

        // Update region symbol
        if (symbolImage != null)
        {
            regionSymbol.sprite = symbolImage;
        }
        else
        {
            Debug.LogWarning("No symbol image found, using default placeholder.");
            regionSymbol.sprite = Resources.Load<Sprite>("Images/DefaultRegionSymbol");
        }
    }
}
