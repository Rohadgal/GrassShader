using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScaleManager : MonoBehaviour
{
    [Header("VFX References")]
    public ParticleSystem[] particleSystems;
    public TrailRenderer[] trailRenderers;
    
    private float[] originalTrailTimes;

    private AnimationCurve[] originalWidthCurves;
    private ParticleSystem.MinMaxCurve[] originalStartSizes;

    private Vector3 previousScale;
    

     void Start()
    {
        originalTrailTimes = new float[trailRenderers.Length];
        originalWidthCurves = new AnimationCurve[trailRenderers.Length];
        originalStartSizes = new ParticleSystem.MinMaxCurve[particleSystems.Length];

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            originalWidthCurves[i] = new AnimationCurve(trailRenderers[i].widthCurve.keys);
            originalTrailTimes[i] = trailRenderers[i].time;
        }

        for (int i = 0; i < particleSystems.Length; i++)
        {
            originalStartSizes[i] = particleSystems[i].main.startSize;
        }

        previousScale = transform.localScale;
        ApplyScale();
    }

    void Update()
    {
        if (transform.localScale != previousScale)
        {
            ApplyScale();
            previousScale = transform.localScale;
        }
    }

    void ApplyScale()
    {
        float scaleFactor = transform.localScale.x; // Assuming uniform scaling

        // Scale particle systems
        for (int i = 0; i < particleSystems.Length; i++)
        {
            var main = particleSystems[i].main;
            ParticleSystem.MinMaxCurve original = originalStartSizes[i];

            ParticleSystem.MinMaxCurve scaledCurve;
            if (original.mode == ParticleSystemCurveMode.TwoConstants)
            {
                scaledCurve = new ParticleSystem.MinMaxCurve(original.constantMin * scaleFactor, original.constantMax * scaleFactor);
            }
            else
            {
                scaledCurve = new ParticleSystem.MinMaxCurve(original.constant * scaleFactor);
            }

            main.startSize = scaledCurve;
        }

        // Scale trail renderers
        for (int i = 0; i < trailRenderers.Length; i++)
        {
            AnimationCurve scaledCurve = new AnimationCurve();
            foreach (Keyframe key in originalWidthCurves[i].keys)
            {
                scaledCurve.AddKey(new Keyframe(key.time, key.value * scaleFactor, key.inTangent, key.outTangent));
            }

            trailRenderers[i].widthCurve = scaledCurve;
            trailRenderers[i].time = originalTrailTimes[i] * scaleFactor;
        }
    }
}
