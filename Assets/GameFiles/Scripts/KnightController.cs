using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum AbilityType
{
    Attack1,
    AttackCombo2,
    AttackCombo3,
    AttackCombo4,
    Lightning,
    GroundSpikes,
    Shield,
    Summon,
    PortalAttack
}

public class KnightController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    private Transform player;
    public GameObject sword;
    public GameObject finishCircle;
    public SvetlesContainer svetlesContainer;

    [Header("HealthSystem")]
    public float maxHP = 3500.0f;
    public float minHP = 0.0f;
    public float currentHP;
    private bool isDeath = false;
    public int svetlesOnDeath = 5000;

    [Header("UI")]
    public Image sliderHP;

    [Header("LungeSettings")]
    [SerializeField] private AnimationCurve lungeCurve;
    [SerializeField] private float lungeDuration = 0.6f;
    [SerializeField] private float lungeDistance = 3f;
    private float lungeTimer;
    private bool isLunging;
    private Vector3 lungeDirection;

    [Header("AISettings")]
    private bool isAttacking = false;
    private NavMeshAgent agent;
    public float attackDistance = 1.5f;

    private bool isBusy;
    private float decisionTimer;
    public float decisionCooldown = 2.0f;

    private float fightTime;

    private Dictionary<AbilityType, float> weights;
    private Dictionary<AbilityType, float> cooldowns;

    private AbilityType lastAbility;
    private int repeatCount;

    [Header("Audio")]
    [Range(0.9f, 1.1f)] public float pitchMin = 0.95f;
    [Range(0.9f, 1.1f)] public float pitchMax = 1.05f;
    public AudioSource knightAudioSource;
    public AudioClip shieldBuffClip;
    public AudioClip lightningCastClip;
    public AudioClip slashSwordClip;

    [Header("References on VFX")]
    public GameObject shieldBuff;
    public GameObject spellGroundParticles;
    public ParticleSystem spellLightning;
    public ParticleSystem slashAttack1;
    public GameObject slash1;
    public Transform spawnSlash1;
    public ParticleSystem slashAttack2;
    public GameObject slash2;
    public Transform spawnSlash2;
    public ParticleSystem slashAttack3;
    public GameObject slash3;
    public Transform spawnSlash3;

    [Header("DeathMagicCircleSettings")]
    public Transform spawnPointCircle;
    public GameObject magicCirclePrefab;
    public float chanceToAttack = 0.95f;
    public float firstChance = 0.95f;

    [Header("SpawnEnemiesSettings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BoxCollider spawnZone;
    [SerializeField] private LayerMask groundLayer;

    [Header("PortalSpell")]
    [SerializeField] private GameObject portalPrefab;
    [SerializeField] private float portalForwardDistance = 2f;
    [SerializeField] private float playerPortalDistance = 2f;
    [SerializeField] private float delayBeforeTeleport = 0.3f;
    [SerializeField] private float delayBeforeAttack = 0.2f;

    [Header("Light")]
    public Light pointLight;
    public MagicLight magicKnightLight;
    public Light[] lightsOnScene;


    [Header("Spell Parameters")]
    public float shieldBuffDuration = 5.0f;
    public float shieldReducedDamage = 0.35f;
    private bool shieldActive = false;

    private bool isSecondPhase = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        currentHP = maxHP;
        InitAI();
        MusicManager.Instance?.RegisterEnemy();
    }

    private void Update()
    {
        if (isDeath)
            return;

        fightTime += Time.deltaTime;
        Debug.DrawRay(spawnSlash1.position, spawnSlash1.forward * 3, Color.red, 2f);

        RotateTowards(player.position);
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(transform.position, player.position);
        agent.isStopped = distance < attackDistance || isLunging || isAttacking;

        if (isBusy)
            return;

        decisionTimer += Time.deltaTime;

        decisionCooldown = Mathf.Lerp(2.0f, 1.0f, fightTime / 60f);

        if (decisionTimer >= decisionCooldown)
        {
            ChooseNextAction();
            decisionTimer = 0f;
        }

        if (isLunging)
        {
            lungeTimer += Time.deltaTime;

            float t = lungeTimer / lungeDuration;
            float curveValue = lungeCurve.Evaluate(t);

            float move = curveValue * lungeDistance * Time.deltaTime;

            transform.position += lungeDirection * move;

            if (t >= 1f)
            {
                isLunging = false;
            }
        }
    }

    private void InitAI()
    {
        weights = new Dictionary<AbilityType, float>()
        {
            { AbilityType.Attack1, 25f },
            { AbilityType.AttackCombo2, 23f },
            { AbilityType.AttackCombo3, 20f },
            { AbilityType.AttackCombo4, 15f },
            { AbilityType.Lightning, 20f },
            { AbilityType.GroundSpikes, 25f },
            { AbilityType.Shield, 17f },
            { AbilityType.Summon, 15f },
            { AbilityType.PortalAttack, 5f }
        };

        cooldowns = new Dictionary<AbilityType, float>();

        foreach (var key in weights.Keys)
            cooldowns[key] = 0f;
    }

    private void ChooseNextAction()
    {
        if (isSecondPhase)
            animator.SetTrigger("Run");
        else
            animator.SetTrigger("Walk");

        List<AbilityType> available = GetAvailableAbilities();

        if (available.Count == 0)
            return;

        AbilityType selected = GetWeightedRandom(available);

        if (selected == lastAbility)
            repeatCount++;
        else
            repeatCount = 0;

        lastAbility = selected;

        ExecuteAbility(selected);
    }

    private List<AbilityType> GetAvailableAbilities()
    {
        List<AbilityType> list = new();

        foreach (var ability in weights.Keys)
        {
            if (Time.time < cooldowns[ability])
                continue;

            if (ability == lastAbility && repeatCount >= 2)
                continue;

            list.Add(ability);
        }

        return list;
    }

    private AbilityType GetWeightedRandom(List<AbilityType> abilities)
    {
        float total = 0f;

        foreach (var a in abilities)
            total += weights[a];

        float random = Random.Range(0, total);

        float current = 0f;

        foreach (var a in abilities)
        {
            current += weights[a];
            if (random <= current)
                return a;
        }

        return abilities[0];
    }

    private void ExecuteAbility(AbilityType ability)
    {
        isBusy = true;

        switch (ability)
        {
            case AbilityType.Attack1:
                animator.SetTrigger("Attack1");
                StartCoroutine(WaitAction(1.2f));
                break;

            case AbilityType.AttackCombo2:
                animator.SetTrigger("Attack2");
                StartCoroutine(WaitAction(1.5f));
                break;

            case AbilityType.AttackCombo3:
                animator.SetTrigger("Attack3");
                StartCoroutine(WaitAction(2.0f));
                break;

            case AbilityType.AttackCombo4:
                animator.SetTrigger("Attack3");
                StartCoroutine(WaitAction(2.2f));
                break;

            case AbilityType.Lightning:
                animator.SetTrigger("CastLightning");
                CastLightningSound();
                cooldowns[ability] = Time.time + 5f;
                StartCoroutine(WaitAction(0.2f));
                break;

            case AbilityType.GroundSpikes:
                animator.SetTrigger("SpellGround");
                SetSpellGround();
                cooldowns[ability] = Time.time + 6f;
                StartCoroutine(WaitAction(1.5f));
                break;

            case AbilityType.Shield:
                animator.SetTrigger("Shield");
                SetShield();
                cooldowns[ability] = Time.time + 10f;
                StartCoroutine(WaitAction(1.5f));
                break;

            case AbilityType.Summon:
                animator.SetTrigger("Enemies");
                SpawnEnemy();
                cooldowns[ability] = Time.time + 15f;
                StartCoroutine(WaitAction(2.5f));
                break;

            case AbilityType.PortalAttack:
                ActivateAbility();
                cooldowns[ability] = Time.time + 20f;
                StartCoroutine(WaitAction(2.0f));
                break;
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EnableSwordCollider(float damageSword)
    {
        sword.GetComponent<Collider>().enabled = true;
        sword.GetComponent<Sword>().damage = damageSword;
    }

    public void DisableSwordCollider()
    {
        sword.GetComponent<Collider>().enabled = false;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private IEnumerator WaitAction(float duration)
    {
        yield return new WaitForSeconds(duration);

        yield return new WaitForSeconds(Random.Range(0.5f, 1.2f));

        isBusy = false;
    }

    public void StartLunge(float lunge)
    {
        lungeDistance = lunge;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < lungeDistance)
            return;

        lungeTimer = 0f;
        isLunging = true;
        lungeDirection = transform.forward;
    }

    public void EndLunge()
    {
        isLunging = false;
        agent.isStopped = false;
    }

    private void SetShield()
    {
        StartCoroutine(ShieldBuff());
    }

    private IEnumerator ShieldBuff()
    {
        yield return new WaitForSeconds(1.1f);

        knightAudioSource.PlayOneShot(shieldBuffClip);
        shieldBuff.SetActive(true);
        shieldActive = true;


        yield return new WaitForSeconds(shieldBuffDuration);

        shieldBuff.SetActive(false);
        shieldActive = false;
    }

    private void SetSpellGround()
    {
        StartCoroutine(CastSpellGroundSpikes());
    }

    private IEnumerator CastSpellGroundSpikes()
    {
        yield return new WaitForSeconds(1.15f);

        RaycastHit hit;

        if (Physics.Raycast(player.position, Vector3.down, out hit, 100f))
        {
            Vector3 spawnPos = hit.point;

            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);

            Instantiate(spellGroundParticles, spawnPos, rotation);
        }

        yield return new WaitForSeconds(3.5f);

        GameObject groundParticles = GameObject.Find("Ground spikes");

        Destroy(groundParticles);
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPoint();

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetSpawnPoint()
    {
        Bounds bounds = spawnZone.bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 5f,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        RaycastHit[] hits = Physics.RaycastAll(randomPoint, Vector3.down, 50f);

        foreach (var hit in hits)
        {
            if (hit.collider != spawnZone)
            {
                return hit.point;
            }
        }

        return randomPoint;
    }

    public void CastLightning()
    {
        spellLightning.Play();
    }

    public void CastLightningSound()
    {
        knightAudioSource.PlayOneShot(lightningCastClip);
    }

    public void ShowSlashAttack1()
    {
        slashAttack1.Play();
        SpawnSlash1();
        knightAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        knightAudioSource.PlayOneShot(slashSwordClip);
    }

    public void SpawnSlash1()
    {
        if (!isSecondPhase)
            return;
        Quaternion rotation = Quaternion.Euler(180f, 0f, 0f);
        Instantiate(slash1, spawnSlash1.position, spawnSlash1.rotation);
    }
    public void SpawnSlash3()
    {
        if (!isSecondPhase)
            return;
        Quaternion rotation = Quaternion.Euler(180f, 0f, 0f);
        Instantiate(slash3, spawnSlash1.position, spawnSlash1.rotation);
    }

    public void ShowSlashAttack2()
    {
        slashAttack2.Play();
        SpawnSlash1();
        knightAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        knightAudioSource.PlayOneShot(slashSwordClip);
    }

    public void ShowSlashAttack3()
    {
        slashAttack3.Play();
        SpawnSlash3();
        knightAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        knightAudioSource.PlayOneShot(slashSwordClip);
    }

    public void RandomCastDeathMagic()
    {
        float random = Random.value;
        Debug.Log(random);

        if (random >= chanceToAttack)
        {
            StartCoroutine(CastDeathMagic());
            chanceToAttack = firstChance;
        }
        else
        {
            chanceToAttack -= 0.05f;
            Debug.Log(chanceToAttack);
        } 
    }

    private IEnumerator CastDeathMagic()
    {
        yield return new WaitForSeconds(0.7f);

        Vector3 spawnPoint = spawnPointCircle.position;

        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);

        Instantiate(magicCirclePrefab, spawnPoint, rotation);
    }

    public void ActivateAbility()
    {
        StartCoroutine(PortalAttackRoutine());
    }

    private IEnumerator PortalAttackRoutine()
    {
        // --- 1. Портал перед боссом ---
        Vector3 bossPortalPos = transform.position + transform.forward * portalForwardDistance;
        GameObject portal1 = Instantiate(portalPrefab, bossPortalPos, Quaternion.identity);

        // --- 2. Портал за игроком ---
        Vector3 directionBehindPlayer = -player.forward; // за спиной
        Vector3 playerPortalPos = player.position + directionBehindPlayer * playerPortalDistance;

        GameObject portal2 = Instantiate(portalPrefab, playerPortalPos, Quaternion.identity);

        yield return new WaitForSeconds(delayBeforeTeleport);

        // --- 3. Телепорт босса ---
        transform.position = portal2.transform.position;

        // --- 4. Разворот к игроку ---
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        yield return new WaitForSeconds(delayBeforeAttack);

        // --- 5. Удаление порталов ---
        Destroy(portal1);
        Destroy(portal2);

        // --- 6. Атака ---
        animator.SetTrigger("Attack1");
    }

    public void TakeDamage(float damage)
    {
        if (isDeath)
            return;

        if (currentHP <= minHP)
        {
            Die();
            return;
        }

        if (currentHP / maxHP < 0.3f)
        {
            if (!isSecondPhase)
                SetSecondPhase();
        }

        if (shieldActive)
            currentHP -= damage * shieldReducedDamage;
        else
            currentHP -= damage * 0.65f;
        UpdateSlider();
    }

    private void SetSecondPhase()
    {
        isSecondPhase = true;
        StartCoroutine(SetAllLightIsRed());
        agent.speed = 7.0f;
        pointLight.color = Color.red;
        Destroy(magicKnightLight);
    }

    private IEnumerator SetAllLightIsRed()
    {
        int i = lightsOnScene.Length;

        for (i = 0; i < lightsOnScene.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);

            lightsOnScene[i].color = Color.red;
        }
    }

    private void Die()
    {
        MusicManager.Instance?.UnregisterEnemy();
        isDeath = true;
        agent.isStopped = true;
        sliderHP.GetComponent<Transform>().parent.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        finishCircle.SetActive(true);
        svetlesContainer.AddSvetles(svetlesOnDeath);
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies)
        {
            enemy.SetDie();
        }
    }

    private void UpdateSlider()
    {
        sliderHP.fillAmount = currentHP / maxHP;
    }
}
