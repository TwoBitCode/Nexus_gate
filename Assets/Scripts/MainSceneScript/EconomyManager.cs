using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Economy Settings")]
    [SerializeField] private int dailySalary = 100;  // Base salary for completing a day
    [SerializeField] private int fineAmount = 20;    // Fine for invalid approvals

    private int finesIncurred;                       // Tracks total fines for the day

    private void Start()
    {
        // Initialize fines and use GameManager for coins
        finesIncurred = 0;
        Debug.Log($"Economy initialized. Starting coins: {GameManager.Instance.coins}");
    }

    public void AddFine()
    {
        finesIncurred += fineAmount;
        Debug.Log($"Fine added: {fineAmount}. Total fines: {finesIncurred}");
    }

    public void AddCoins(int amount)
    {
        // Update total coins in GameManager
        GameManager.Instance.AddCoins(amount);
        Debug.Log($"Added {amount} coins. Total coins: {GameManager.Instance.coins}");
    }

    public int CalculateEndOfDayEarnings(bool isSuccessfulDay)
    {
        if (!isSuccessfulDay) return 0; // If the game is over, no earnings for the day
        int earnings = dailySalary - finesIncurred;
        Debug.Log($"End of day earnings calculated. Salary: {dailySalary}, Fines: {finesIncurred}, Earnings: {earnings}");

        return earnings;
    }

    public void ResetDailyEarnings()
    {
        finesIncurred = 0;
        Debug.Log("Daily earnings reset.");
    }

    public int GetFines()
    {
        return finesIncurred;
    }

    public int GetTotalCoins()
    {
        return GameManager.Instance.coins; // Get coins from GameManager
    }

    public int GetDailySalary()
    {
        return dailySalary;
    }
}
