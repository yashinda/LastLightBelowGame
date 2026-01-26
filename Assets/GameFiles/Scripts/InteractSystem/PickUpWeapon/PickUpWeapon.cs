using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    public WeaponChanger weaponContainer;
    public Transform weaponHolder;
    public int indexPickUpWeapon = 1;
    public GameObject ammoboxes;
    public AudioClip pickUpClip;

    private void Update()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent = weaponHolder;
            transform.position = new Vector3(-0.109300002f, 0.0909999982f, 0.829699993f);
            weaponContainer.AddWeapon(gameObject);
            weaponContainer.ActivateWeapon(indexPickUpWeapon);
            ammoboxes.SetActive(true);
            GetComponent<Revolver>().enabled = true;
            GetComponent<Animator>().enabled = true;
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().PlayOneShot(pickUpClip);
            this.enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
