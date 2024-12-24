using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    [Header("Player Data")]
    public int reputation = 100;  // Player's reputation across all days
    public int coins = 0;         // Player's coins across all days

    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    public void AdjustReputation(int amount)
    {
        reputation += amount;
        reputation = Mathf.Clamp(reputation, 0, 100); // Keep reputation within bounds
        Debug.Log($"Reputation adjusted: {amount}. Current reputation: {reputation}");
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"Coins added: {amount}. Current coins: {coins}");
    }

    public void DeductCoins(int amount)
    {
        coins -= amount;
        if (coins < 0) coins = 0;
        Debug.Log($"Coins deducted: {amount}. Current coins: {coins}");
    }
}
