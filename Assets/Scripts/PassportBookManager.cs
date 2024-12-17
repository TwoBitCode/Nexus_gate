using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassportBookManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject bookPanel;
    public TextMeshProUGUI regionName;
    public Image regionSymbol;

    [Header("Passport Data")]
    public string[] regionNames;
    public Sprite[] regionSymbols;

    private int currentPage = 0;

    private void Start()
    {
        if (regionNames.Length != regionSymbols.Length)
        {
            Debug.LogError("Passport data mismatch: regionNames and regionSymbols arrays must have the same length.");
            return;
        }
        UpdatePage();
    }

    public void OpenBook()
    {
        if (regionNames.Length == 0 || regionSymbols.Length == 0)
        {
            Debug.LogError("Passport book data is empty. Check the inspector.");
            return;
        }

        bookPanel.SetActive(true);
        currentPage = 0;
        UpdatePage();
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
    }

    public void NextPage()
    {
        if (currentPage < regionNames.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        if (currentPage >= 0 && currentPage < regionNames.Length && currentPage < regionSymbols.Length)
        {
            regionName.text = regionNames[currentPage];
            regionSymbol.sprite = regionSymbols[currentPage];
        }
        else
        {
            Debug.LogError("Invalid passport page data.");
        }
    }
}
