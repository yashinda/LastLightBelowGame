using UnityEngine;

public class LootSvetles : MonoBehaviour, IInteractable
{
    public SvetlesContainer svetlesContainer;
    [SerializeField] private int amount;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private InteractionData interactionTake;

    public void Interact()
    {
        svetlesContainer.AddSvetles(amount);
        audioSource.PlayOneShot(clip);
        Destroy(gameObject);
    }

    public InteractionType GetInteractionType() => InteractionType.Take;
    public string GetInteractionDescription() => interactionTake.description.GetLocalizedString();
}
