using UnityEngine;

public class PickUpRocket : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    public WeaponChanger weaponContainer;
    public GameObject rocket;
    public int indexPickUpWeapon = 3;
    public GameObject ammoboxes;
    public AudioClip pickUpClip;
    public AudioSource audioSource;

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponContainer.AddWeapon(rocket);
            weaponContainer.ActivateWeapon(indexPickUpWeapon);

            if (ammoboxes != null)
                ammoboxes.SetActive(true);

            audioSource.PlayOneShot(pickUpClip);
            Destroy(gameObject);
        }
    }
}
