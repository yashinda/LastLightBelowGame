using UnityEngine;

public class FinishCircle : MonoBehaviour
{
    public int echoSvetlesAmount = 300;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EchoSvetles.Add(echoSvetlesAmount);
            LevelStateController.Instance.CompleteLevel();
            Destroy(gameObject);
        }
    }
}
