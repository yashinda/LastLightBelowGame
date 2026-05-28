using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Interactions/InteractionData")]
public class InteractionData : ScriptableObject
{
    public LocalizedString description;
}
