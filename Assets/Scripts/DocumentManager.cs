using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DocumentManager : MonoBehaviour
{
    public TextMeshProUGUI documentText; // The text displaying all document details
    public Image applicantImage; // Displays the applicant's face

    private string[] humanNames = { "John Doe", "Jane Smith", "Ali Khan", "Lara Moon" };
    private string[] humanOrigins = { "Earth", "Mars", "Jupiter" };

    private string[] alienNames = { "Zarqa", "Nebulo", "Xilra", "Vetra" };
    private string[] alienOrigins = { "Zarquinia", "Nebulon IV", "Andromeda Prime" };

    private string[] documentTypes = { "Entry Permit", "Travel Pass", "Work Visa" };

    private Sprite[] humanImages;
    private Sprite[] alienImages;

    private void Start()
    {
        // Load images from the Resources folder
        humanImages = LoadImagesFromResources("Images/Humans");
        alienImages = LoadImagesFromResources("Images/Aliens");

        Debug.Log($"Loaded {humanImages?.Length ?? 0} human images.");
        Debug.Log($"Loaded {alienImages?.Length ?? 0} alien images.");

        // Generate the first document
        GenerateDocument();
    }

    private Sprite[] LoadImagesFromResources(string resourceFolder)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(resourceFolder);

        if (sprites.Length == 0)
        {
            Debug.LogError($"No images found in folder: {resourceFolder}");
        }
        return sprites;
    }

    public void GenerateDocument()
    {
        string name, origin, documentType;
        Sprite faceImage = null;

        bool isHuman = Random.value > 0.5f;

        if (isHuman && humanImages.Length > 0)
        {
            name = humanNames[Random.Range(0, humanNames.Length)];
            origin = humanOrigins[Random.Range(0, humanOrigins.Length)];
            faceImage = humanImages[Random.Range(0, humanImages.Length)];
        }
        else if (!isHuman && alienImages.Length > 0)
        {
            name = alienNames[Random.Range(0, alienNames.Length)];
            origin = alienOrigins[Random.Range(0, alienOrigins.Length)];
            faceImage = alienImages[Random.Range(0, alienImages.Length)];
        }
        else
        {
            Debug.LogError("No images available for applicant!");
            return;
        }

        documentType = documentTypes[Random.Range(0, documentTypes.Length)];
        int age = Random.Range(16, 85);

        // Update the document text with all details
        documentText.text = $"Name: {name}\n" +
                            $"Age: {age}\n" +
                            $"Origin: {origin}\n" +
                            $"Document: {documentType}";

        // Update the applicant image
        applicantImage.sprite = faceImage;
    }
}
