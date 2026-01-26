using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponChanger : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();

    private int currentWeaponIndex = 0;

    public void ActivateWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count)
            return;

        for (int i = 0; i < weapons.Count; i++)
            weapons[i].SetActive(i == index);

        currentWeaponIndex = index;
    }

    public void AddWeapon(GameObject weapon)
    {
        weapons.Add(weapon);
    }

    private void OnFirstWeapon() => ActivateWeapon(0);
    private void OnSecondWeapon() => ActivateWeapon(1);
    private void OnThirdWeapon() => ActivateWeapon(2);
    private void OnFourthWeapon() => ActivateWeapon(3);
    private void OnFifthWeapon() => ActivateWeapon(4);
}