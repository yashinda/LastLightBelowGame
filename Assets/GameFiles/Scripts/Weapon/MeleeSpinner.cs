using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class MeleeSpinner : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset actionAsset;
    private InputAction attackAction;

    [Header("Spin (degrees/sec)")]
    [SerializeField] private Transform bladesTransform;
    [SerializeField] private float maxSpinSpeed = 720.0f;
    [SerializeField] private float spinAccel = 1800.0f;
    [SerializeField] private float spinDecay = 900.0f;
    [SerializeField] private float minEffectiveSpin = 60.0f;

    [Header("Damage")]
    [SerializeField] private float damagePerSecond = 10.0f;
    [SerializeField] private bool debugLogHits = false;

    private float currentSpinSpeed = 0.0f;

    private HashSet<EnemyBase> enemiesInRange = new HashSet<EnemyBase>();
    private Dictionary<EnemyBase, float> damageAccumulators = new Dictionary<EnemyBase, float>();

    private Collider triggerCollider;

    private void Awake()
    {
        attackAction = actionAsset.FindActionMap("Player").FindAction("Attack");
        triggerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        bool isHolding = attackAction != null && attackAction.IsPressed();

        if (isHolding)
        {
            currentSpinSpeed += spinAccel * Time.deltaTime;
            if (currentSpinSpeed > maxSpinSpeed) currentSpinSpeed = maxSpinSpeed;
        }
        else
        {
            currentSpinSpeed -= spinDecay * Time.deltaTime;
            if (currentSpinSpeed < 0f) currentSpinSpeed = 0f;
        }

        if (Mathf.Abs(currentSpinSpeed) > 0.001f && bladesTransform != null)
        {
            bladesTransform.Rotate(Vector3.forward, currentSpinSpeed * Time.deltaTime, Space.Self);
        }

        if (currentSpinSpeed >= minEffectiveSpin)
        {
            float dmgThisFrame = damagePerSecond * Time.deltaTime;

            foreach (var enemy in enemiesInRange)
            {
                if (enemy == null) continue;

                if (!damageAccumulators.TryGetValue(enemy, out float acc))
                    acc = 0f;

                acc += dmgThisFrame;
                int whole = (int)acc;
                if (whole > 0)
                {
                    damageAccumulators[enemy] = acc - whole;
                    enemy.TakeDamage(whole);
                    if (debugLogHits)
                        Debug.Log($"MeleeSpinner: hit {enemy.name} for {whole} dmg (spin {currentSpinSpeed:F1} deg/s)");
                }
                else
                {
                    damageAccumulators[enemy] = acc;
                }
            }
        }
        else
        {
            if (damageAccumulators.Count > 0)
                damageAccumulators.Clear();
        }

        if (enemiesInRange.Count > 0)
        {
            var toRemove = new List<EnemyBase>();
            foreach (var e in enemiesInRange)
            {
                if (e == null) toRemove.Add(e);
            }
            foreach (var r in toRemove)
            {
                enemiesInRange.Remove(r);
                damageAccumulators.Remove(r);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (!other.CompareTag("Enemy"))
            return;

        var enemy = other.gameObject.GetComponentInParent<EnemyBase>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
            if (!damageAccumulators.ContainsKey(enemy))
                damageAccumulators[enemy] = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        if (!other.CompareTag("Enemy"))
            return;

        var enemy = other.gameObject.GetComponentInParent<EnemyBase>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
            damageAccumulators.Remove(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }

    public float GetCurrentSpinSpeed() => currentSpinSpeed;
    public bool IsSpinning() => currentSpinSpeed >= minEffectiveSpin;
    public void ForceStop()
    {
        currentSpinSpeed = 0f;
        enemiesInRange.Clear();
        damageAccumulators.Clear();
    }
}
