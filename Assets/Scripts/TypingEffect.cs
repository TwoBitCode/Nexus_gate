using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections;
using UnityEngine.SceneManagement; // For scene loading

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // TextMeshProUGUI component
    public string[] lines; // Array of text lines
    public float typingSpeed = 0.05f; // Delay between letters
    public AudioSource typingSound; // AudioSource for typing sound
    public AudioClip typingClip; // Assign typing sound here

    private int lettersPerSound = 20; // Play sound every X letters

    void Start()
    {
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (string line in lines)
        {
            textComponent.text = ""; // Clear text at start of line
            int letterCount = 0;

            foreach (char letter in line.ToCharArray())
            {
                textComponent.text += letter; // Add letter to text
                letterCount++;

                // Trigger the typing sound on the first letter and every few letters
                if (letterCount % lettersPerSound == 0 || letterCount == 1)
                {
                    PlayTypingSound();
                }

                yield return new WaitForSecondsRealtime(typingSpeed); // Wait before next letter
            }

            // Stop sound at the end of the line
            StopTypingSound();

            // Wait before showing the next line
            yield return new WaitForSecondsRealtime(1f);
        }

        OnComplete(); // Load next scene after all lines
    }

    void PlayTypingSound()
    {
        if (typingSound != null && typingClip != null)
        {
            typingSound.PlayOneShot(typingClip);
        }
    }

    void StopTypingSound()
    {
        if (typingSound != null)
        {
            typingSound.Stop();
        }
    }

    void OnComplete()
    {
        Debug.Log("Typing Effect Complete!");

        // Load the Main Game Scene
        SceneManager.LoadScene("DailyMessageScene");
    }
}
