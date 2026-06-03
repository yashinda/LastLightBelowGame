using UnityEngine;

public class TriggerChangeStatue : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip triggerClip;
    public GameObject firstStatue;
    public GameObject secondStatue;
    public GameObject firstChain;
    public GameObject secondChain;
    public GameObject thirdChain;
    public GameObject skelet;
    public GameObject book;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        audioSource.PlayOneShot(triggerClip);
        firstStatue.SetActive(false);
        firstChain.SetActive(false);
        secondChain.SetActive(false);
        thirdChain.SetActive(false);
        skelet.SetActive(true);
        book.SetActive(true);
        secondStatue.SetActive(true);
        Destroy(gameObject);
    }
}
