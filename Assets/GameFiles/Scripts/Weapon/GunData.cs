using UnityEngine;

[CreateAssetMenu(fileName = "New GunData", menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;

    public LayerMask targetLayerMask;

    
}
