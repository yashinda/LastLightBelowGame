using UnityEngine;

public class OpenShop : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject panelShop;
    [SerializeField] private LevelStateController gameManager;
    public void Interact()
    {
        panelShop.SetActive(true);
        gameManager.PlayerChoosesUpgrade();
    }

    public InteractionType GetInteractionType() => InteractionType.Use;

    public string GetInteractionDescription() => "открыть магазин";
}
