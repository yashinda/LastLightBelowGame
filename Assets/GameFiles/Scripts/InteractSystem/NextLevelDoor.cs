using UnityEngine;

public class NextLevelDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        LevelStateController.Instance.LoadNextLevel();
    }

    public InteractionType GetInteractionType() => InteractionType.Use;

    public string GetInteractionDescription() => "начать следующий уровень";
}
