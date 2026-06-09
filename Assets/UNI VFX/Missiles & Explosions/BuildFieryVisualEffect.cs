using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BuildFieryVisualEffect : MonoBehaviour
{
    private VisualEffect effect;
    public float delay = 2.0f;

    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        StartCoroutine(ShowEffect());
    }

    private IEnumerator ShowEffect()
    {
        effect.SendEvent("buildup");
        
        yield return new WaitForSeconds(delay);
        
        effect.SendEvent("hit");
    }
}
