using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoCountText;
    [SerializeField] private Transform weaponHolder;

    private void Update()
    {
        Gun activeGun = GetActiveGun();

        if (activeGun != null)
        {
            ammoCountText.text = $"{activeGun.CurrentAmmo}";
        }
        else
        {
            ammoCountText.text = "∞";
        }
    }

    private Gun GetActiveGun()
    {
        for (int i = 0; i < weaponHolder.childCount; i++)
        {
            Transform child = weaponHolder.GetChild(i);

            if (!child.gameObject.activeInHierarchy)
                continue;

            return child.GetComponent<Gun>();
        }

        return null;
    }
}
