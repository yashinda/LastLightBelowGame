using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    public void UpdateBarValue(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
    }
}
