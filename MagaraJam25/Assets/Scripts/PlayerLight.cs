using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private float startingOuterRadius = 30f;
    [SerializeField] private float endingOuterRadius = 3f;
    [SerializeField] private float innerRadiusMultiplier = 0.6f;
    [SerializeField] private float sightCountdownMax = 15f;

    private float sightCountdown;
    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = startingOuterRadius;
        light2D.pointLightInnerRadius = startingOuterRadius * innerRadiusMultiplier;

        sightCountdown = sightCountdownMax;
    }

    private void Update()
    {
        UpdateSight();
    }

    private void UpdateSight()
    {
        if (Player.Instance.IsCarrying())
        {
            sightCountdown = sightCountdownMax;
            light2D.pointLightOuterRadius = startingOuterRadius;
            light2D.pointLightInnerRadius = light2D.pointLightOuterRadius * innerRadiusMultiplier;
        }
        if (light2D.pointLightOuterRadius <= endingOuterRadius) return;

        light2D.pointLightOuterRadius = startingOuterRadius * (sightCountdown / sightCountdownMax);
        light2D.pointLightInnerRadius = light2D.pointLightOuterRadius * innerRadiusMultiplier;

        sightCountdown -= Time.deltaTime;
    }
}
