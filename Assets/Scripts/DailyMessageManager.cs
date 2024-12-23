using UnityEngine;
using TMPro;
using System.Collections;

public class DailyMessageManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI officerDialogue; // Dialogue box
    public string[] messages;               // Array of messages

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;       // Speed of typing effect
    public float messageWaitTime = 2f;     // Wait time between messages

    [Header("Audio")]
    public AudioSource officerSound;       // Audio source for typing sound

    [Header("Transition Settings")]
    public CanvasGroup dailyMessageGroup;  // Canvas Group for Daily Message UI
    public CanvasGroup tutorialGroup;      // Canvas Group for Tutorial UI
    private const float FadeDuration = 1f; // Duration of UI fade transitions

    void Start()
    {
        StartCoroutine(DisplayMessages());
    }

    private IEnumerator DisplayMessages()
    {
        foreach (string message in messages)
        {
            officerDialogue.text = string.Empty; // Clear the text
            officerSound.Play();                 // Start the sound

            foreach (char letter in message)
            {
                officerDialogue.text += letter;
                yield return new WaitForSeconds(typingSpeed); // Typing delay
            }

            officerSound.Stop();                 // Stop the sound after line finishes
            yield return new WaitForSeconds(messageWaitTime); // Wait before the next message
        }

        // Transition to Tutorial UI after all messages are displayed
        yield return StartCoroutine(TransitionToTutorial());
    }

    private IEnumerator TransitionToTutorial()
    {
        yield return StartCoroutine(FadeOutUI(dailyMessageGroup)); // Fade out Daily Message UI
        yield return StartCoroutine(FadeInUI(tutorialGroup));      // Fade in Tutorial UI
        Debug.Log("Tutorial phase started!");
    }

    private IEnumerator FadeOutUI(CanvasGroup ui)
    {
        yield return FadeUI(ui, 1f, 0f);
        ui.interactable = false;
        ui.blocksRaycasts = false;
    }

    private IEnumerator FadeInUI(CanvasGroup ui)
    {
        ui.gameObject.SetActive(true); // Ensure it's active before fading in
        yield return FadeUI(ui, 0f, 1f);
        ui.interactable = true;
        ui.blocksRaycasts = true;
    }

    private IEnumerator FadeUI(CanvasGroup ui, float startAlpha, float endAlpha)
    {
        for (float t = 0; t < FadeDuration; t += Time.deltaTime)
        {
            ui.alpha = Mathf.Lerp(startAlpha, endAlpha, t / FadeDuration);
            yield return null;
        }
        ui.alpha = endAlpha;
    }
}
