using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float lifeTime = 1.0f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
