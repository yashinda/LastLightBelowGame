using UnityEngine;

public class EmissionPulse : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color emissionColor = Color.yellow;

    [SerializeField] private float minIntensity = 8.5f;
    [SerializeField] private float maxIntensity = 9.0f;
    [SerializeField] private float speed = 1.0f;

    private Material material;

    private void Awake()
    {
        material = targetRenderer.material;

        material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        material.SetColor("_EmissionColor", emissionColor * intensity);
    }
}
