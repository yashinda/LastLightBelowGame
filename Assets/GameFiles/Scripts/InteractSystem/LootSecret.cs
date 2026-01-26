using UnityEngine;

public class LootSecret : MonoBehaviour, IInteractable
{
    public StatisticsLevel statistic;
    public SvetlesContainer svetlesContainer;
    [SerializeField] private int svetlesCount;

    public void Interact()
    {
        statistic.AddSecret();
        svetlesContainer.AddSvetles(svetlesCount);

        Destroy(gameObject);
    }

    public string GetInteractionDescription() => "взять секрет";

    public InteractionType GetInteractionType() => InteractionType.Take;
}
