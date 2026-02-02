using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public LevelStateController gameManager;

    [Header("MovementParameters")]
    public float speed = 4.0f;
    public float usualSpeed = 6.0f;
    public float shiftSpeed = 10.0f;
    private Vector3 playerVelocity;
    public bool sprintPressed = false;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("LookingParameters")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 75.0f;

    [SerializeField] float currentPitch = 0.0f;

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
    public bool evasionPressed = false;

    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera cameraPlayer;

    #region Unity Methods
    private void Update()
    {
        if (!playerHealth.PlayerDead && !gameManager.IsPaused && gameManager.CurrentState != LevelState.ChooseUpgrade)
        {
            MoveUpdate();
            LookUpdate();
        }
    }

    #endregion

    #region Controller Methods
    private void MoveUpdate()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.Normalize();
        motion *= speed;

        HandleGravityAndJump();
        characterController.Move((motion + playerVelocity) * Time.deltaTime);
        HandleSprint();
    }

    private void HandleGravityAndJump()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2.0f;
        else
            jumpPressed = false;

        playerVelocity.y += gravity * Time.deltaTime;

        if (jumpPressed && characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
    }

    private void HandleSprint()
    {
        if (sprintPressed && characterController.isGrounded)
        {
            speed = shiftSpeed;
        }
        else
        {
            speed = usualSpeed;
        }
    }

    private void LookUpdate()
    {
        Vector2 input = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);
        CurrentPitch -= input.y;

        cameraPlayer.transform.localRotation = Quaternion.Euler(CurrentPitch, 0, 0);
        transform.Rotate(Vector3.up * input.x);
    }

    #endregion
}