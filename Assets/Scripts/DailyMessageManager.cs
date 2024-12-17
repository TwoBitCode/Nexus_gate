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
    }
}
