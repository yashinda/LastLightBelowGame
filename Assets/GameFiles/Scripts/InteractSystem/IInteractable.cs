using UnityEngine;

public enum InteractionType
{
    None,
    Open,
    Take,
    Use
}

public interface IInteractable
{
    void Interact();
    InteractionType GetInteractionType();
    string GetInteractionDescription();
}
