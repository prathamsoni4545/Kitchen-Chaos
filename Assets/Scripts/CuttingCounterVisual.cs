using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT_TRIGGER = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private ParticleSystem cutParticle; // optional particle for effect
    [SerializeField] private AudioSource cutSfx; // optional audio for each cut

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (cuttingCounter != null)
        {
            cuttingCounter.OnCut += CuttingCounter_OnCut;
        }
    }

    private void OnDisable()
    {
        if (cuttingCounter != null)
        {
            cuttingCounter.OnCut -= CuttingCounter_OnCut;
        }
    }

    private void CuttingCounter_OnCut(object sender, EventArgs e)
    {
        if (animator != null)
        {
            animator.SetTrigger(CUT_TRIGGER);
        }

        if (cutParticle != null)
        {
            cutParticle.Play();
        }

        if (cutSfx != null)
        {
            cutSfx.Play();
        }

    }
}
