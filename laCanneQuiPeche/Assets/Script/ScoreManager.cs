using UnityEngine;

public static class ScoreManager
{
    private static int score = 0;

    public static void SetScore(int newScore)
    {
        score = newScore;
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }

    public static bool CanAfford(int cost)
    {
        return score >= cost;
    }

    public static bool Purchase(int cost)
    {
        if (CanAfford(cost))
        {
            score -= cost;
            return true;
        }
        return false;
    }
}