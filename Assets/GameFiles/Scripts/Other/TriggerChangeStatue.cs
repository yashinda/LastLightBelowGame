using UnityEngine;

public class TriggerChangeStatue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip triggerClip;
    public GameObject firstStatue;
    public GameObject secondStatue;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        audioSource.PlayOneShot(triggerClip);
        firstStatue.SetActive(false);
        secondStatue.SetActive(true);
        Destroy(gameObject);
    }
}
