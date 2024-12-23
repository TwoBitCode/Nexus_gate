using UnityEngine;
using TMPro;
using System.Collections;

public class DailyMessageManager : MonoBehaviour
{
    public TextMeshProUGUI officerDialogue;  // Dialogue box
    public string[] messages;                // Array of messages
    public float typingSpeed = 0.05f;        // Speed of typing effect
    public float messageWaitTime = 2f;       // Wait time between messages

    public AudioSource officerSound;         // Audio source for typing sound

    // New Variables for Transition
    public CanvasGroup dailyMessageGroup;    // Canvas Group for Daily Message UI
    public CanvasGroup tutorialGroup;        // Canvas Group for Tutorial UI

    void Start()
    {
        StartCoroutine(DisplayMessages());
    }

    IEnumerator DisplayMessages()
    {
        foreach (string message in messages)
        {
            officerDialogue.text = "";    // Clear the text
            officerSound.Play();          // Start the sound

            foreach (char letter in message.ToCharArray())
            {
                officerDialogue.text += letter;
                yield return new WaitForSeconds(typingSpeed); // Typing delay
            }

            officerSound.Stop();          // Stop the sound after line finishes
            yield return new WaitForSeconds(messageWaitTime); // Wait before next message
        }

        // Transition to Tutorial UI after all messages are displayed
        StartCoroutine(TransitionToTutorial());
    }

    IEnumerator TransitionToTutorial()
    {
        // Fade out Daily Message UI
        yield return StartCoroutine(FadeOutUI(dailyMessageGroup));

        // Fade in Tutorial UI
        yield return StartCoroutine(FadeInUI(tutorialGroup));

        Debug.Log("Tutorial phase started!");
    }

    private IEnumerator FadeOutUI(CanvasGroup ui)
    {
        float duration = 1f; // Duration of fade
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            ui.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        ui.alpha = 0;
        ui.interactable = false;
        ui.blocksRaycasts = false;
    }

    private IEnumerator FadeInUI(CanvasGroup ui)
    {
        ui.alpha = 0;
        ui.gameObject.SetActive(true); // Ensure it's active before fading in
        float duration = 1f; // Duration of fade
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            ui.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        ui.alpha = 1;
        ui.interactable = true;
        ui.blocksRaycasts = true;
    }
}
