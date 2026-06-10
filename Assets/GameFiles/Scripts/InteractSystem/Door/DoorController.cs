using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private InteractionData interactionOpen;

    private bool isOpen = false;

    public InteractionType GetInteractionType()
    {
        return isOpen ? InteractionType.None : InteractionType.Open;
    }

    public string GetInteractionDescription() => interactionOpen.description.GetLocalizedString();

    public void Interact()
    {
        if (isOpen)
            return;

        isOpen = true;
        animator.SetBool("Open", true);
        audioSource.PlayOneShot(clip);

        this.enabled = false;
    }
}
