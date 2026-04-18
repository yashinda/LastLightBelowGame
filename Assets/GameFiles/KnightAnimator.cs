using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class KnightAnimator : MonoBehaviour
{

    [Header("References")]
    public Animator animator;
    private Transform player;

    [Header("LungeSettings")]
    [SerializeField] private AnimationCurve lungeCurve;
    [SerializeField] private float lungeDuration = 0.6f;
    [SerializeField] private float lungeDistance = 3f;
    private float lungeTimer;
    private bool isLunging;
    private Vector3 lungeDirection;

    [Header("AISettings")]
    private NavMeshAgent agent;
    public float attackDistance = 1.5f;

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
    public ParticleSystem slashAttack2;
    public ParticleSystem slashAttack3;

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


    [Header("Spell Parameters")]
    public float shieldBuffDuration = 5.0f;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
    }

    private void Update()
    {
        RotateTowards(player.position);
        agent.SetDestination(player.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Shield");
            SetShield();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("SpellGround");
            SetSpellGround();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("CastLightning");
            CastLightningSound();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            animator.SetTrigger("Attack1");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            animator.SetTrigger("Attack2");

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("Attack3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.SetTrigger("Enemies");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
            ActivateAbility();

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
                agent.isStopped = false;
            }
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < attackDistance)
            agent.isStopped = true;
    }

    public void StartLunge()
    {
        agent.isStopped = true;
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

        yield return new WaitForSeconds(shieldBuffDuration);

        shieldBuff.SetActive(false);
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
        knightAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        knightAudioSource.PlayOneShot(slashSwordClip);
    }

    public void ShowSlashAttack2()
    {
        slashAttack2.Play();
        knightAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        knightAudioSource.PlayOneShot(slashSwordClip);
    }

    public void ShowSlashAttack3()
    {
        slashAttack3.Play();
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
}
