using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    private void Awake()
    {
        globalLight.enabled = false;
    }
}
