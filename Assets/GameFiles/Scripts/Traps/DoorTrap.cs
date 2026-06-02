using System;
using System.Collections;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    public Animator doorAnimator;
    private bool trapIsTriggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (trapIsTriggered)
            return;
        
        if (other.tag == "Player")
        {
            StartCoroutine(AnimateDoor());
        }
    }

    private IEnumerator AnimateDoor()
    {
        trapIsTriggered = true;
        doorAnimator.SetBool("Close", true);

        yield return new WaitForSeconds(5.0f);

        doorAnimator.SetBool("Open", true);
    }
}
