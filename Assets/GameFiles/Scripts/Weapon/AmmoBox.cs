using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private ushort ammo = 30;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gun[] guns = other.GetComponentsInChildren<Gun>(true);

            foreach (Gun gun in guns)
            {
                if (gun.ammoType == ammoType && gun.CurrentAmmo < gun.MagSize)
                {
                    gun.AddAmmo(ammo); 
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
