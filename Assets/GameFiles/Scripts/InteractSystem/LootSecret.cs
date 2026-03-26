using UnityEngine;

public enum BuffCharacteristic
{
    Health,
    Psi,
    Armor,
    Revolver,
    Shotgun,
    Rifle
}

public class LootSecret : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject levelController;
    public StatisticsLevel statistic;
    public SvetlesContainer svetlesContainer;
    [SerializeField] private int svetlesCount;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Transform player;
    [SerializeField] private BuffCharacteristic characteristic;
    [SerializeField] private int buffCount;

    public void Interact()
    {
        statistic.AddSecret();
        svetlesContainer.AddSvetles(svetlesCount);
        audioSource.PlayOneShot(audioClip);

        Destroy(gameObject);

        switch (characteristic)
        {
            case BuffCharacteristic.Health:
                player.GetComponent<PlayerHealth>().IncreaseMaxHP(buffCount);
                break;
            case BuffCharacteristic.Psi:
                levelController.GetComponent<PsySystem>().IncreaseMaxPsi(buffCount);
                break;
            case BuffCharacteristic.Armor:
                player.GetComponent<PlayerHealth>().GetArmor(buffCount);
                break;
            case BuffCharacteristic.Revolver:
                player.GetComponentInChildren<Revolver>().IncreaseDamage(buffCount);
                break;
            case BuffCharacteristic.Shotgun:
                player.GetComponentInChildren<Shotgun>().IncreaseDamage(buffCount);
                break;
            case BuffCharacteristic.Rifle:
                player.GetComponentInChildren<Rifle>().IncreaseDamage(buffCount);
                break;
        }
    }

    public string GetInteractionDescription() => "взять секрет";

    public InteractionType GetInteractionType() => InteractionType.Take;
}
