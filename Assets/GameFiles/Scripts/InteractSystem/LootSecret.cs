using UnityEngine;

public class LootSecret : MonoBehaviour, IInteractable
{
    public StatisticsLevel statistic;
    public SvetlesContainer svetlesContainer;
    [SerializeField] private int svetlesCount;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    public void Interact()
    {
        statistic.AddSecret();
        svetlesContainer.AddSvetles(svetlesCount);
        audioSource.PlayOneShot(audioClip);

        Destroy(gameObject);
    }

    public string GetInteractionDescription() => "взять секрет";

    public InteractionType GetInteractionType() => InteractionType.Take;
}
