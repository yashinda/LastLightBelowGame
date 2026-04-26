using UnityEngine;

public class WeaponAnimatorInput : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;

    [Header("Current Weapon")]
    private Animator currentAnimator;

    [Header("Settings")]
    public float animationSpeed = 2.0f;
    public float defaultAnimationSpeed = 1.0f;

    private void Update()
    {
        if (currentAnimator == null)
            return;

        float targetSpeed = playerController.moveInput != Vector2.zero
            ? animationSpeed
            : defaultAnimationSpeed;

        currentAnimator.SetFloat("AnimationSpeed", targetSpeed);
    }

    public void SetCurrentWeaponAnimator(Animator animator)
    {
        currentAnimator = animator;
    }
}