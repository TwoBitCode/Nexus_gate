using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Economy Settings")]
    [SerializeField] private int startingCoins = 0;   // Initial coins for the player
    [SerializeField] private int dailySalary = 100;  // Base salary for completing a day
    [SerializeField] private int fineAmount = 20;    // Fine for invalid approvals

    private int currentCoins;                        // Tracks player's total coins
    private int finesIncurred;                       // Tracks total fines for the day

    private void Start()
    {
        currentCoins = startingCoins;
        finesIncurred = 0;
        Debug.Log($"Economy initialized. Starting coins: {currentCoins}");
    }

    public void AddFine()
    {
        finesIncurred += fineAmount;
        Debug.Log($"Fine added: {fineAmount}. Total fines: {finesIncurred}");
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log($"Added {amount} coins. Total coins: {currentCoins}");
    }

    public int CalculateEndOfDayEarnings(bool isSuccessfulDay)
    {
        if (!isSuccessfulDay) return 0; // No earnings if the day is unsuccessful
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
        return currentCoins;
    }

    public int GetDailySalary()
    {
        return dailySalary;
    }
}
