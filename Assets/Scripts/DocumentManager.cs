using UnityEngine;
using TMPro;

public class DocumentManager : MonoBehaviour
{
    public TextMeshProUGUI documentText;

    private string[] names = { "John Doe", "Jane Smith", "Ali Khan", "Lara Moon" };
    private string[] origins = { "Earth", "Mars", "Venus", "Jupiter" };
    private string[] documentTypes = { "Entry Permit", "Travel Pass", "Work Visa" };

    void Start()
    {
        // Automatically generate a document when the game starts
        GenerateDocument();
    }

    public void GenerateDocument()
    {
        string name = names[Random.Range(0, names.Length)];
        string origin = origins[Random.Range(0, origins.Length)];
        string docType = documentTypes[Random.Range(0, documentTypes.Length)];
        int age = Random.Range(16, 65); // Generate a random valid age between 16 and 65

        // Update the document text UI
        documentText.text = $"Name: {name}\nAge: {age}\nOrigin: {origin}\nDocument: {docType}";
    }
}
