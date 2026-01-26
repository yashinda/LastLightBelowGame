using UnityEngine;

public class FinishCircle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelStateController.Instance.CompleteLevel();
            Destroy(gameObject);
        }
    }
}
