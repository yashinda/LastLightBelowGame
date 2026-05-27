using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]
public class InteractionData : ScriptableObject
{
    public LocalizedString description;
}
