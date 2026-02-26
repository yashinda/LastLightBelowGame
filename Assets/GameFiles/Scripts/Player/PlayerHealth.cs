using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LevelStateController sceneController;

    [Header("Health")]
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 0f;
    [SerializeField] private int armor = 0;

    [Header("Damage Feedback")]
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip damageClip;


    private bool isDead;
    private bool invincible;

    public bool PlayerDead => isDead;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public int Armor => armor;
    public bool IsInvincible => invincible;

    public void TakeDamage(float damage)
    {
        if (invincible || isDead)
            return;

        if (armor > 0)
        {
            armor -= (int)Mathf.Ceil(damage);
        }
        else
        {
            currentHealth -= damage;
        }
        
        if (currentHealth <= minHealth)
        {
            Die();
        }
    }

    public void TakePsyDamage(float damage)
    {
        if (invincible || isDead)
            return;

        currentHealth -= damage;
        if (currentHealth <= minHealth)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void SetInvincible(bool value)
    {
        invincible = value;
    }

    public void IncreaseMaxHP(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }

    public void GetArmor(int amount)
    {
        armor += amount;
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

    public void SetArmor(int amount)
    {
        armor = amount;
    }

    private void Die()
    {
        isDead = true;
        LevelStateController.Instance.PlayerDied();
    }
}
