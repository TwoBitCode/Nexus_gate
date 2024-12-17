using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypingEffect : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI textComponent; // Text display

    [Header("Typing Settings")]
    [SerializeField] private string[] lines;  // Array of text lines
    [SerializeField] private float typingSpeed = 0.05f; // Delay between letters
    [SerializeField] private float lineWaitTime = 1f; // Delay between lines
    [SerializeField] private int lettersPerSound = 20; // Sound trigger frequency

    [Header("Audio")]
    public AudioSource typingSound; // Typing sound source
    public AudioClip typingClip;    // Typing sound clip

    [Header("Scene Management")]
    [SerializeField] private string nextSceneName = "DailyMessageScene"; // Next scene

    private void Start()
    {
        if (!ValidateComponents()) return;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (string line in lines)
        {
            textComponent.text = ""; // Clear text at start of line
            int letterCount = 0;

            foreach (char letter in line.ToCharArray())
            {
                textComponent.text += letter; // Add letter to text
                letterCount++;

                // Play typing sound at the start and every few letters
                if (letterCount % lettersPerSound == 0 || letterCount == 1)
                {
                    PlayTypingSound();
                }

                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            StopTypingSound();
            yield return new WaitForSecondsRealtime(lineWaitTime);
        }

        OnComplete();
    }

    private void PlayTypingSound()
    {
        if (typingSound != null && typingClip != null)
        {
            typingSound.PlayOneShot(typingClip);
        }
    }

    private void StopTypingSound()
    {
        if (typingSound != null)
        {
            typingSound.Stop();
        }
    }

    private void OnComplete()
    {
        Debug.Log("Typing Effect Complete!");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }

    private bool ValidateComponents()
    {
        if (textComponent == null || lines == null || lines.Length == 0)
        {
            Debug.LogError("Text Component or Lines not assigned!");
            return false;
        }

        if (typingSound == null)
        {
            Debug.LogWarning("Typing sound source is missing.");
        }

        return true;
    }
}
