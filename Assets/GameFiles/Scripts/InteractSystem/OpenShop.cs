using UnityEngine;

public class OpenShop : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject panelShop;
    [SerializeField] private LevelStateController gameManager;
    [SerializeField] private InteractionData interactionShop;
    public void Interact()
    {
        panelShop.SetActive(true);
        gameManager.PlayerChoosesUpgrade();
    }

    public InteractionType GetInteractionType() => InteractionType.Use;

    public string GetInteractionDescription() => interactionShop.description.GetLocalizedString();
}
