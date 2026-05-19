using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WeaponChanger : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();
    public List<GameObject> iconsWeapons = new List<GameObject>();

    private int currentWeaponIndex = 0;
    public WeaponAnimatorInput weaponAnimator;

    private Vector2 scrollInput;
    
    private void Start()
    {
        ActivateWeapon(0);
    }
    
    private void NextWeapon()
    {
        if (weapons.Count == 0)
            return;

        currentWeaponIndex++;

        if (currentWeaponIndex >= weapons.Count)
            currentWeaponIndex = 0;

        ActivateWeapon(currentWeaponIndex);
    }

    private void PreviousWeapon()
    {
        if (weapons.Count == 0)
            return;

        currentWeaponIndex--;

        if (currentWeaponIndex < 0)
            currentWeaponIndex = weapons.Count - 1;

        ActivateWeapon(currentWeaponIndex);
    }

    public void ActivateWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count)
            return;

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == index);
            iconsWeapons[i].SetActive(i == index);
            weaponAnimator.SetCurrentWeaponAnimator(weapons[index].GetComponent<Animator>());
        }

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

    private void OnScrollWeapon(InputValue value)
    {
        scrollInput = value.Get<Vector2>();

        if (scrollInput.y < 0)
        {
            NextWeapon();
        }
        else if (scrollInput.y > 0)
        {
            PreviousWeapon();
        }
    }
}