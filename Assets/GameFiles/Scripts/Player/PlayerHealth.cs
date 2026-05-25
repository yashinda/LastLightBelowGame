using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private LevelStateController sceneController;

    [Header("Health")]
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float minHealth = 0f;
    [SerializeField] private int armor = 0;

    [Header("Reincarnation")] 
    [SerializeField] private GameObject invincibleEffect;

    [Header("Damage Feedback")] 
    [SerializeField] private Material bloodEffect;
    [SerializeField] private float maxIntencity = 4.0f;
    [SerializeField] private float fadeInSpeed = 12.0f;
    [SerializeField] private float fadeOutSpeed = 3.0f;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip damageClipFirst;
    [SerializeField] private AudioClip damageClipSecond;
    [Range(0.9f, 1.1f)] [SerializeField] private float pitchMin; 
    [Range(0.9f, 1.1f)] [SerializeField] private float pitchMax; 
    private Coroutine bloodEffectCoroutine;

    private bool isDead;
    private bool invincible;

    public bool PlayerDead => isDead;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public int Armor => armor;
    public bool IsInvincible => invincible;

    private void Start()
    {
        var boostHP = SkillsAfterDeath.BoostMaxHp;
        var boostArmor = SkillsAfterDeath.BoostArmor;
        bloodEffect.SetFloat("_ScreenIntencity", 0.0f);
        currentHealth = maxHealth + boostHP;
        armor += boostArmor;
    }

    private void Update()
    {
        if (currentHealth <= 30.0f)
            ShowBloodEffect();
    }

    public void TakeDamage(float damage)
    {
        if (invincible || isDead)
            return;

        ShowBloodEffect();
        PlayAudioHit();
        
        if (armor > 0)
        {
            armor -= (int)Mathf.Ceil(damage);
            if (armor < 0)
                armor = 0;
        }
        else
        {
            currentHealth -= damage;
        }
        
        if (currentHealth <= minHealth)
        {
            int reincarnationAmount = SkillsAfterDeath.ReincarnationAmount;
            if (reincarnationAmount > 0)
            {
                currentHealth = maxHealth / 2.0f;
                StartCoroutine(ReincarnationCoroutine());
                SkillsAfterDeath.RemoveReincarnation();
            }
            else
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

    private void PlayAudioHit()
    {
        playerAudioSource.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
        float randomValue = UnityEngine.Random.value;
        if (randomValue > 0.5f)
            playerAudioSource.PlayOneShot(damageClipFirst);
        else
            playerAudioSource.PlayOneShot(damageClipSecond);
    }

    private IEnumerator ReincarnationCoroutine()
    {
        invincible = true;
        invincibleEffect.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        invincible = false;
        invincibleEffect.SetActive(false);
    }

    private void Die()
    {
        isDead = true;
        LevelStateController.Instance.PlayerDied();
    }

    public void ShowBloodEffect()
    {
        if (bloodEffectCoroutine != null)
            StopCoroutine(bloodEffectCoroutine);

        bloodEffectCoroutine = StartCoroutine(BloodEffectCoroutine());
    }

    private IEnumerator BloodEffectCoroutine()
    {
        float current = bloodEffect.GetFloat("_ScreenIntencity");

        while (current < maxIntencity)
        {
            current = Mathf.MoveTowards(current, maxIntencity, fadeInSpeed * Time.deltaTime);
            
            bloodEffect.SetFloat("_ScreenIntencity", current);
            
            yield return null;
        }

        while (current > 0.0f)
        {
            current = Mathf.MoveTowards(current, 0.0f, fadeOutSpeed * Time.deltaTime);
            
            bloodEffect.SetFloat("_ScreenIntencity", current);
            
            yield return null;
        }
        
        bloodEffect.SetFloat("_ScreenIntencity", 0.0f);
    }
}
