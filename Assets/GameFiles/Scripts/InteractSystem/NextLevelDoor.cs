using UnityEngine;

public class NextLevelDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        LevelStateController.Instance.ConfirmNextLevel();
    }

    public InteractionType GetInteractionType() => InteractionType.Use;

    public string GetInteractionDescription() => "начать следующий уровень";
}
