using UnityEngine;

public class RandomLightIntencity : MonoBehaviour
{
    private Light light;
    [SerializeField] private float minIntensity = 5.0f;
    [SerializeField] private float maxIntensity = 7.0f;
    [SerializeField] private float changeInterval = 0.1f;

    private float timer;
    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            light.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }
    }
}
