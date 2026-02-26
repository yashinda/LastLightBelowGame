using UnityEngine;

public class PsySystem : MonoBehaviour
{
    [Header("Game Manager")]
    public LevelStateController gameManager;

    [Header("Player Settings")]
    public Transform player;
    public PlayerHealth playerHealth;

    [Header("Psy Settings")]
    public float psyAmount = 100.0f;
    public float maxPsyAmount = 100.0f;
    public float minPsyAmount = 0.0f;
    public float psyChangeRate = 1.0f;
    public bool madnessActive = false;
    public float psyAmountForMadness = 50.0f;
    public float damage = 1.0f;

    private void Update()
    {
        UpdatePsyAmount();
        CheckMadness();
    }

    private void UpdatePsyAmount()
    {
        if (playerHealth.PlayerDead || gameManager.CurrentState != LevelState.Playing) 
            return;

        if (IsInAnyLight(player.position))
            psyAmount = Mathf.MoveTowards(psyAmount, maxPsyAmount, psyChangeRate * Time.deltaTime);
        else
            psyAmount = Mathf.MoveTowards(psyAmount, minPsyAmount, psyChangeRate * Time.deltaTime);
    }

    private void CheckMadness()
    {
        if (playerHealth.PlayerDead || gameManager.CurrentState != LevelState.Playing)
            return;

        madnessActive = psyAmount <= psyAmountForMadness;
        if (madnessActive && !IsInAnyLight(player.position))
            Madness();
    }

    private void Madness()
    {
        if (playerHealth.PlayerDead || gameManager.CurrentState != LevelState.Playing)
            return;

        playerHealth.TakePsyDamage(damage * Time.deltaTime);
    }

    public void IncreaseMaxPsi(float amount)
    {
        maxPsyAmount += amount;
        psyAmount = maxPsyAmount;
    }

    public void SetMaxPsyAmount(float amount)
    {
        maxPsyAmount = amount;
        psyAmount = maxPsyAmount;
    }

    private bool IsInAnyLight(Vector3 playerPos)
    {
        Light[] lights = FindObjectsOfType<Light>();

        foreach (Light light in lights)
        {
            if (!light.enabled)
                continue;

            if (!light.TryGetComponent<MagicLight>(out _))
                continue;

            if (Vector3.Distance(light.transform.position, playerPos) <= light.range)
                return true;
        }

        return false;
    }
}
