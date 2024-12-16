using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassportManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI passportText; // Displays applicant's passport details
    public Image applicantImage; // Displays applicant's face

    private string[] alienNames = { "Zarqa Elion", "Nebulo Xel", "Quorin Arak", "Vetra Shiran", "Xilra Talos" };
    private string[] alienOrigins = { "Zarquinia", "Nebulon IV", "Andromeda Prime", "Galva-Theta", "Xyron-9" };

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
        Debug.Log($"Loaded {alienImages?.Length ?? 0} alien images.");

        // Verify loaded images
        if (alienImages.Length == 0)
        {
            Debug.LogError("Failed to load images. Make sure images exist in Resources/Images/Aliens.");
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
        if (passportText == null || applicantImage == null)
        {
            Debug.LogError("Missing UI elements. Please check PassportManager references.");
            return;
        }

        string name, origin;
        Sprite faceImage = null;
        int age;

        float randomType = Random.value;

        if (randomType < 0.8f && alienImages != null && alienImages.Length > 0)
        {
            // Generate a valid alien applicant
            name = alienNames[Random.Range(0, alienNames.Length)];
            origin = alienOrigins[Random.Range(0, alienOrigins.Length)];
            faceImage = alienImages[Random.Range(0, alienImages.Length)];
            age = Random.Range(minAlienAge - 5, maxAlienAge + 5);
        }
        else
        {
            // Generate a fake applicant with a random image
            name = fakeNames[Random.Range(0, fakeNames.Length)];
            origin = fakeOrigins[Random.Range(0, fakeOrigins.Length)];

            // Use random alien images for fake applicants
            if (alienImages != null && alienImages.Length > 0)
            {
                faceImage = alienImages[Random.Range(0, alienImages.Length)];
            }
            else
            {
                Debug.LogWarning("No alien images available, loading default image.");
                faceImage = Resources.Load<Sprite>("Images/DefaultImage"); // Load a default placeholder image
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
    }

}
