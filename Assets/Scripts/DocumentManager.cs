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
        // Load images from Assets/Images/Humans and Assets/Images/Aliens
        humanImages = LoadImagesFromFolder("Assets/Images/Humans");
        alienImages = LoadImagesFromFolder("Assets/Images/Aliens");

        Debug.Log($"Loaded {humanImages?.Length ?? 0} human images.");
        Debug.Log($"Loaded {alienImages?.Length ?? 0} alien images.");

        // Generate the first document
        GenerateDocument();
    }

    private Sprite[] LoadImagesFromFolder(string folderPath)
    {
        string[] textureFiles = System.IO.Directory.GetFiles(folderPath, "*.png");

        if (textureFiles.Length == 0)
        {
            Debug.LogError($"No images found in folder: {folderPath}");
            return new Sprite[0];
        }

        Sprite[] sprites = new Sprite[textureFiles.Length];

        for (int i = 0; i < textureFiles.Length; i++)
        {
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData = System.IO.File.ReadAllBytes(textureFiles[i]);
            texture.LoadImage(fileData);

            // Create a sprite from the texture
            sprites[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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
        documentText.text = $"<b>Name:{name}\n" +
                            $"<b>Age:{age}\n" +
                            $"<b>Origin:{origin}\n" + // No extra space after "Origin:"
                            $"<b>Document: {documentType}";

        // Update the applicant image
        applicantImage.sprite = faceImage;


    }
}
