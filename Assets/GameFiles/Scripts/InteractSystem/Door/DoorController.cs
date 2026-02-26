using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private bool isOpen = false;

    public InteractionType GetInteractionType() => InteractionType.Open;

    public string GetInteractionDescription() => "открыть дверь";

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
