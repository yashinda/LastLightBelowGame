using UnityEngine;

public class SvetlesContainer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StatisticsLevel statistic;

    [SerializeField] private int svetlesCount = 0;
    public int CurrentSvetles => svetlesCount;

    public void AddSvetles(int svetles)
    {
        svetlesCount += svetles;
        if (statistic != null)
            statistic.AddSvetlesToStatictics(svetles);
    }

    public void SpendSvetles(int amount)
    {
        svetlesCount = Mathf.Max(0, svetlesCount - amount);
    }

    public void SetSvetlesAmount(int value)
    {
        svetlesCount = value;
    }
}
