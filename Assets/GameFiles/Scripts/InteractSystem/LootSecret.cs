using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject imageSecret;
    [SerializeField] private InteractionData interactionTake;

    public void Interact()
    {
        statistic.AddSecret();
        svetlesContainer.AddSvetles(svetlesCount);
        audioSource.PlayOneShot(audioClip);
        imageSecret.SetActive(true);

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
                player.GetComponentInChildren<Revolver>(true).IncreaseDamage(buffCount);
                break;
            case BuffCharacteristic.Shotgun:
                player.GetComponentInChildren<Shotgun>(true).IncreaseDamage(buffCount);
                break;
            case BuffCharacteristic.Rifle:
                player.GetComponentInChildren<Rifle>(true).IncreaseDamage(buffCount);
                break;
        }
    }
    public string GetInteractionDescription() => interactionTake.description.GetLocalizedString();

    public InteractionType GetInteractionType() => InteractionType.Take;
}
