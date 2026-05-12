using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public LevelStateController gameManager;

    [Header("Movement")]
    public float usualSpeed = 10.0f;
    public float shiftSpeed = 14.0f;

    [Header("Quake Movement")]
    public float groundAcceleration = 16f;
    public float airAcceleration = 3f;
    public float groundFriction = 5f;
    public float airControl = 0.35f;

    [Header("Gravity")]
    public float gravity = -20.0f;
    public float jumpForce = 8.0f;

    [Header("Stun")]
    public float stunSpeed = 0.0f;
    private bool inStun = false;
    public GameObject stunPanel;

    [Header("LookingParameters")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 75.0f;

    [SerializeField] private float currentPitch = 0.0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
        }
    }

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;

    public bool jumpPressed = false;
    public bool sprintPressed = false;
    public bool evasionPressed = false;

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera cameraPlayer;

    private Vector3 velocity;

    #region Unity Methods

    private void Update()
    {
        if (!playerHealth.PlayerDead &&
            !gameManager.IsPaused &&
            gameManager.CurrentState != LevelState.ChooseUpgrade)
        {
            MoveUpdate();
            LookUpdate();
        }
    }

    #endregion

    #region Movement

    private void MoveUpdate()
    {
        if (inStun)
            return;

        bool grounded = characterController.isGrounded;

        Vector3 wishDir =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        wishDir.Normalize();

        float targetSpeed = sprintPressed
            ? shiftSpeed
            : usualSpeed;

        if (grounded)
        {
            GroundMove(wishDir, targetSpeed);

            if (jumpPressed)
            {
                velocity.y = jumpForce;
                jumpPressed = false;
            }
        }
        else
        {
            AirMove(wishDir, targetSpeed);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void GroundMove(Vector3 wishDir, float targetSpeed)
    {
        ApplyFriction();

        Accelerate(
            wishDir,
            targetSpeed,
            groundAcceleration
        );

        if (velocity.y < 0)
            velocity.y = -2f;
    }

    private void AirMove(Vector3 wishDir, float targetSpeed)
    {
        Accelerate(
            wishDir,
            targetSpeed,
            airAcceleration
        );

        AirControlMovement(wishDir);
    }

    private void Accelerate(
        Vector3 wishDir,
        float wishSpeed,
        float acceleration)
    {
        float currentSpeed =
            Vector3.Dot(velocity, wishDir);

        float addSpeed =
            wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return;

        float accelSpeed =
            acceleration *
            Time.deltaTime *
            wishSpeed;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        velocity += wishDir * accelSpeed;
    }

    private void ApplyFriction()
    {
        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0;

        float speed = horizontalVelocity.magnitude;

        if (speed < 0.1f)
            return;

        float drop = speed * groundFriction * Time.deltaTime;

        float newSpeed = speed - drop;

        if (newSpeed < 0)
            newSpeed = 0;

        newSpeed /= speed;

        velocity.x *= newSpeed;
        velocity.z *= newSpeed;
    }

    private void AirControlMovement(Vector3 wishDir)
    {
        if (Mathf.Abs(moveInput.y) < 0.001f)
            return;

        float ySpeed = velocity.y;
        velocity.y = 0;

        float speed = velocity.magnitude;

        if (speed < 0.001f)
        {
            velocity.y = ySpeed;
            return;
        }

        velocity.Normalize();

        float dot =
            Vector3.Dot(velocity, wishDir);

        float control = 32f * airControl * dot * dot * Time.deltaTime;

        if (dot > 0)
        {
            velocity.x = velocity.x * speed + wishDir.x * control;

            velocity.z = velocity.z * speed + wishDir.z * control;

            velocity.Normalize();
        }

        velocity *= speed;
        velocity.y = ySpeed;
    }

    #endregion

    #region Look

    private void LookUpdate()
    {
        float sensitivity =
            PlayerPrefs.GetFloat(
                "SensitivityValue",
                1.0f
            );

        Vector2 input = new Vector2(
            lookInput.x *
            (lookSensitivity.x * sensitivity),

            lookInput.y *
            (lookSensitivity.y * sensitivity)
        );

        CurrentPitch -= input.y;

        cameraPlayer.transform.localRotation =
            Quaternion.Euler(CurrentPitch, 0, 0);

        transform.Rotate(Vector3.up * input.x);
    }

    #endregion

    #region Stun

    public void Stun(float duration)
    {
        Debug.Log("Оглушение");
        StartCoroutine(PlayerInStun(duration));
    }

    private IEnumerator PlayerInStun(float duration)
    {
        inStun = true;

        if (stunPanel != null)
            stunPanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        inStun = false;

        if (stunPanel != null)
            stunPanel.SetActive(false);
    }

    #endregion
}