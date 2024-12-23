using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject approveButton;         // Approve button
    public GameObject denyButton;            // Deny button
    public GameObject passportBookButton;    // Passport Book button
    public GameObject passportIcon;          // Passport Icon

    public TextMeshProUGUI tooltip;          // Tooltip text

    [Header("Highlight Effects")]
    public GameObject approveHighlight;      // Highlight for approve button
    public GameObject denyHighlight;         // Highlight for deny button
    public GameObject bookHighlight;         // Highlight for passport book button
    public GameObject passportHighlight;     // Highlight for passport icon

    public float typingSpeed = 0.05f;        // Typing speed for tooltip text

    void Start()
    {
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        // Step 1: Explain Approve Button
        yield return ShowTooltip("This is the Approve button. Use it to allow travelers entry into the station.", approveHighlight);

        // Step 2: Explain Deny Button
        yield return ShowTooltip("This is the Deny button. Use it to reject travelers who fail to meet the entry criteria.", denyHighlight);

        // Step 3: Explain Passport Book Button
        yield return ShowTooltip("This is the Passport Book. Open it to review entry rules and regulations.", bookHighlight);

        // Step 4: Explain Passport Icon
        yield return ShowTooltip("This is the Passport Icon. It represents the applicant's passport. Use it to check their information and decide if they meet the rules.", passportHighlight);

        // End of Tutorial
        tooltip.text = "You're ready! Start processing travelers now.";
        yield return new WaitForSeconds(2f);

        Debug.Log("Tutorial completed.");
    }

    private IEnumerator ShowTooltip(string message, GameObject highlight)
    {
        // Enable the highlight effect for the current element
        highlight.SetActive(true);

        // Update the tooltip text with typing effect
        tooltip.text = ""; // Clear text before typing
        foreach (char letter in message.ToCharArray())
        {
            tooltip.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Wait for typing effect
        }

        // Wait for the player to read the message
        yield return new WaitForSeconds(2f);

        // Disable the highlight effect
        highlight.SetActive(false);
    }
}
