using UnityEngine;

public class OpenLattice : MonoBehaviour
{
    public GameObject encounter;
    public Animator animatorLattice;
    public AudioSource sourceLattice;
    public AudioClip clipLattice;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animatorLattice.SetBool("Open", true);
            sourceLattice.PlayOneShot(clipLattice);
            encounter.SetActive(true);
            Destroy(gameObject);
        }
    }
}
