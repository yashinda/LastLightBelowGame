using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool trapIsTriggered = false;

    [SerializeField] private Animator trapAnimator;

    [SerializeField] private Transform trapTransform;
    [SerializeField] private Vector3 sizeTrap = new Vector3(0.01f, 0.007f, 0.01f);
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private float damage = 15.0f;
    [SerializeField] private float timeToActivation = 1.0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (trapIsTriggered)
            return;

        trapIsTriggered = true;
        StartCoroutine(ActivateTrap());
    }

    private void OnTriggerExit(Collider other)
    {
        trapIsTriggered = false;
    }

    private IEnumerator ActivateTrap()
    {
        yield return new WaitForSeconds(timeToActivation);
        trapAnimator.SetTrigger("Trigger");
        yield return new WaitForSeconds(0.2f);
        TrapHitPlayer();
    }

    public void TrapHitPlayer()
    {
        Collider[] colliders = Physics.OverlapBox(trapTransform.position, sizeTrap);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                var playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                    playerHealth.TakeDamage(damage);
            }

            if (collider.CompareTag("Enemy"))
            {
                var enemy = collider.GetComponent<EnemyBase>();
                if (enemy != null)
                    enemy.TakeDamage(damage);
            }                
        }
    }
}
