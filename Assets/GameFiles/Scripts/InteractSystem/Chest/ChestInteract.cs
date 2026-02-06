using UnityEngine;

public class ChestInteract : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Animator animator;
    public GameObject loot;
    public AudioSource audioSource;
    public AudioClip clip;

    private bool isOpened = false;
    public void Interact()
    {
        if (!isOpened)
        {
            OpenChest();
        }
        else
        {
            return;
        }
    }

    private void OpenChest()
    {
        animator.SetBool("Open", true);
        audioSource.PlayOneShot(clip);
        if (loot != null)
            loot.SetActive(true);
    }

    public InteractionType GetInteractionType()
    {
        return isOpened ? InteractionType.None : InteractionType.Open;
    }

    public string GetInteractionDescription() => "открыть сундук";
}
