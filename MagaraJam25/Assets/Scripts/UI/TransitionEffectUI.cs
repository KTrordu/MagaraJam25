using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffectUI : MonoBehaviour
{
    public static TransitionEffectUI Instance { get; private set; }

    private Animator animator;

    private const string TRANSITION = "Transition";

    private void Awake()
    {
        Instance = this;

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        RoomExit.OnRoomExitTriggered += RoomExit_OnRoomExitTriggered;
    }

    private void RoomExit_OnRoomExitTriggered(object sender, System.EventArgs e)
    {
        HandleTransitionEffect();
    }

    private void HandleTransitionEffect()
    {
        animator.SetTrigger(TRANSITION);
    }
}
