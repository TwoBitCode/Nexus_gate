using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CanvasGroup dailyMessageGroup;  // Canvas Group for Daily Message UI
    public CanvasGroup tutorialGroup;     // Canvas Group for Tutorial UI

    public void ShowDailyMessage()
    {
        dailyMessageGroup.alpha = 1;
        dailyMessageGroup.interactable = true;
        dailyMessageGroup.blocksRaycasts = true;
    }

    public void HideDailyMessage()
    {
        dailyMessageGroup.alpha = 0;
        dailyMessageGroup.interactable = false;
        dailyMessageGroup.blocksRaycasts = false;
    }

    public void ShowTutorial()
    {
        tutorialGroup.alpha = 1;
        tutorialGroup.interactable = true;
        tutorialGroup.blocksRaycasts = true;
    }

    public void HideTutorial()
    {
        tutorialGroup.alpha = 0;
        tutorialGroup.interactable = false;
        tutorialGroup.blocksRaycasts = false;
    }

    public void TransitionToTutorial()
    {
        // Example transition
        HideDailyMessage();
        ShowTutorial();
    }
}
