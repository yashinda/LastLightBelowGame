using UnityEngine;

public class NextLevelDoor : MonoBehaviour, IInteractable
{
    [SerializeField] InteractionData interactionData;
    public void Interact()
    {
        LevelStateController.Instance.ConfirmNextLevel();
    }

    public InteractionType GetInteractionType() => InteractionType.NextLevel;

    public string GetInteractionDescription() => interactionData.description.GetLocalizedString();
}
