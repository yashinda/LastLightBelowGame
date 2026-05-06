using UnityEngine;

public class ActivateBarrels : MonoBehaviour
{
    public GameObject barrel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            barrel.SetActive(true);
            Destroy(this);
        }
    }
}
