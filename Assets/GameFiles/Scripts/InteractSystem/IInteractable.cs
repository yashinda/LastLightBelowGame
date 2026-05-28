using UnityEngine;

public enum InteractionType
{
    None,
    Open,
    Take,
    Use,
    NextLevel
}

public interface IInteractable
{
    void Interact();
    InteractionType GetInteractionType();
    string GetInteractionDescription();
}
