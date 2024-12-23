using UnityEngine;

public class UIManager : MonoBehaviour
{
    // References to UI Canvas Groups
    public CanvasGroup dailyMessageGroup;  // Canvas Group for Daily Message UI
    public CanvasGroup tutorialGroup;     // Canvas Group for Tutorial UI

    // Shows a CanvasGroup by enabling its properties
    public void ShowUI(CanvasGroup group)
    {
        if (group == null)
        {
            Debug.LogError("CanvasGroup is null. Ensure all CanvasGroups are assigned in the inspector.");
            return;
        }

        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    // Hides a CanvasGroup by disabling its properties
    public void HideUI(CanvasGroup group)
    {
        if (group == null)
        {
            Debug.LogError("CanvasGroup is null. Ensure all CanvasGroups are assigned in the inspector.");
            return;
        }

        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    // Transitions from the Daily Message UI to the Tutorial UI
    public void TransitionToTutorial()
    {
        HideUI(dailyMessageGroup);
        ShowUI(tutorialGroup);
    }
}
