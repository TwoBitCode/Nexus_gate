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
        UpdatePage();
    }

    public void OpenBook()
    {
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
        if (regionNames.Length > currentPage && regionSymbols.Length > currentPage)
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
