using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LevelStateController gameManager;
    [SerializeField] private GameObject panelSettings;
    #region Input Handling

    void OnMove(InputValue value)
    {
        playerController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        playerController.lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
            playerController.jumpPressed = true;
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
            playerController.sprintPressed = true;
        else
            playerController.sprintPressed = false;
    }

    void OnEvade(InputValue value)
    {
        if (value.isPressed)
            playerController.evasionPressed = true;
    }

    void OnPause(InputValue value)
    {
        if (value.isPressed && !panelSettings.activeSelf)
            gameManager.TogglePause();
        else
            panelSettings.SetActive(false);
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion
}