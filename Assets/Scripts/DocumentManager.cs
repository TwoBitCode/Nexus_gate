using UnityEngine;
using TMPro;

public class DocumentManager : MonoBehaviour
{
    public TextMeshProUGUI documentText;

    private string[] humanNames = { "John Doe", "Jane Smith", "Ali Khan", "Lara Moon" };
    private string[] alienNames = { "Zor'ath", "K'Laar", "Xenova", "Thrak", "Velquar" };
    private string[] humanOrigins = { "Earth", "Mars", "Venus", "Jupiter" };
    private string[] alienOrigins = { "Zarquinia", "Nebulon IV", "Andromeda Prime", "Sirius B", "Betelgeuse" };
    private string[] documentTypes = { "Entry Permit", "Travel Pass", "Work Visa", "Diplomatic Clearance", "Trade License" };

    void Start()
    {
        // Automatically generate a document when the game starts
        GenerateDocument();
    }

    public void GenerateDocument()
    {
        bool isAlien = Random.Range(0, 2) == 0; // 50% chance of alien or human

        string name = isAlien
            ? alienNames[Random.Range(0, alienNames.Length)]
            : humanNames[Random.Range(0, humanNames.Length)];
        string origin = isAlien
            ? alienOrigins[Random.Range(0, alienOrigins.Length)]
            : humanOrigins[Random.Range(0, humanOrigins.Length)];
        string docType = documentTypes[Random.Range(0, documentTypes.Length)];
        int age = isAlien ? Random.Range(200, 1000) : Random.Range(16, 65);

        documentText.text = $"Name: {name}\n" +
                            $"Age: {age}\n" +
                            $"Origin: {origin}\n" +
                            $"Document: {docType}";
    }
}
