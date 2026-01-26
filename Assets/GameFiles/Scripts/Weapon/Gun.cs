using UnityEngine;
using UnityEngine.InputSystem;

public enum AmmoType
{
    Revolver,
    Shotgun,
    AssaultRifle,
    Rocket
}

public abstract class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private LevelStateController gameManager;

    [SerializeField] protected private string gunName;
    public AmmoType ammoType;
    [SerializeField] protected private Transform spawnBulletTransform;
    [SerializeField] private InputActionAsset actionAsset;
    private InputAction attackAction;


    [SerializeField] private int currentAmmo;
    [SerializeField] private float nextTimeToFire;
    public ParticleSystem muzzleFlash;
    public Animator animator;

    [Header("Fire Config")]
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected private int shootingRange;
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private int magSize;
    [SerializeField] private bool autoFire;

    [Header("Audio")]
    [SerializeField] protected AudioSource shotAudioSource;
    [SerializeField] protected AudioClip shotClip;
    [SerializeField, Range(0.9f, 1.1f)] protected float pitchMin = 0.95f;
    [SerializeField, Range(0.9f, 1.1f)] protected float pitchMax = 1.05f;

    public int CurrentAmmo => currentAmmo;
    public int MagSize => magSize;

    public int Damage => damage;
   

    private void Start()
    {
        attackAction = actionAsset.FindActionMap("Player").FindAction("Attack");
    }

    public virtual void Update()
    {
        if (playerHealth.PlayerDead || gameManager.CurrentState != LevelState.Playing)
            return;

        if (autoFire)
        {
            if (attackAction.IsPressed())
                TryShoot();
        }
        else
        {
            if (attackAction.triggered)
                TryShoot();
        }
    }

    private void TryShoot()
    {
        if (currentAmmo <= 0)
            Debug.Log($"В {gunName} нет патронов");

        if (Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + (1 / fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        currentAmmo--;
        PlayShotSound();
        Shoot();
        if (animator != null)
            animator.SetTrigger("Shot");
        if (muzzleFlash != null)
            muzzleFlash.Play();
    }

    public void AddAmmo(ushort ammo)
    { 
        currentAmmo += ammo;
        if (currentAmmo >= magSize)
        {
            currentAmmo = magSize;
        }
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    protected void PlayShotSound()
    {
        if (shotAudioSource == null || shotClip == null)
            return;

        shotAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        shotAudioSource.PlayOneShot(shotClip);
    }

    protected abstract void Shoot();
}