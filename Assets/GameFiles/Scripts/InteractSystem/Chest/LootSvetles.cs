using UnityEngine;

public class LootSvetles : MonoBehaviour, IInteractable
{
    public SvetlesContainer svetlesContainer;
    [SerializeField] private int amount;

    public void Interact()
    {
        svetlesContainer.AddSvetles(amount);
        Destroy(gameObject);
    }

    public InteractionType GetInteractionType() => InteractionType.Take;
    public string GetInteractionDescription() => "подобрать светлы";
}
