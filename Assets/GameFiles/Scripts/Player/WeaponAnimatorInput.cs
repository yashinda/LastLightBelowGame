using UnityEngine;

public class WeaponAnimatorInput : MonoBehaviour
{
    public Animator animatorShotgun;
    public Animator animatorRevolver;
    public Animator animatorRifle;
    public Animator animatorRocket;
    public PlayerController playerController;
    public float animationSpeed = 2.0f;
    public float defaultAnimationSpeed = 1.0f;

    private void Update()
    {
        if (playerController.moveInput != Vector2.zero)
        {
            animatorShotgun.SetFloat("AnimationSpeed", animationSpeed);
        }
        else
        {
            animatorShotgun.SetFloat("AnimationSpeed", defaultAnimationSpeed);
        }
    }
} 
