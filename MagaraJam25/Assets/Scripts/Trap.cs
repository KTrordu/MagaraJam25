using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public event EventHandler OnTrapPressedByPlayer;

    public enum TrapType
    {
        None,
        A,
        B
    }

    [SerializeField] TrapType type;
    [SerializeField] private PressurePlate pressurePlate;

    private bool isDisabledWithPressurePlate = false;
    private bool isDisabledByCorpse = false;

    private Collider2D trapCollider;

    private void Awake()
    {
        trapCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerPickedUpCorpse += Player_OnPlayerPickedUpCorpse;
        Player.Instance.OnPlayerDroppedCorpse += Player_OnPlayerDroppedCorpse;

        if (pressurePlate != null)
        {
            pressurePlate.OnPressurePlatePressed += PressurePlate_OnPressurePlatePressed;
            pressurePlate.OnPressurePlateReleased += PressurePlate_OnPressurePlateReleased;
        }
    }

    private void PressurePlate_OnPressurePlateReleased(object sender, EventArgs e)
    {
        isDisabledWithPressurePlate = false;
    }

    private void PressurePlate_OnPressurePlatePressed(object sender, EventArgs e)
    {
        isDisabledWithPressurePlate = true;
    }

    private void Player_OnPlayerDroppedCorpse(object sender, EventArgs e)
    {
        if (trapCollider.IsTouching(Corpse.Instance.GetCorpseCollider()))
        {
            isDisabledByCorpse = true;
        }
    }

    private void Player_OnPlayerPickedUpCorpse(object sender, EventArgs e)
    {
        HandleCorpseRemoval();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Corpse>() != null && !Player.Instance.IsCarrying())
        {
            isDisabledByCorpse = true;
        }

        if (collision.gameObject.GetComponent<Corpse>() != null && Player.Instance.IsCarrying())
        {
            HandleCorpseRemoval();
        }

        if (collision.gameObject.GetComponent<Player>() != null && !IsDisabled())
        {
            OnTrapPressedByPlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleCorpseRemoval()
    {
        if (isDisabledByCorpse)
        {
            isDisabledByCorpse = false;
        }
    }

    public bool IsDisabled() => isDisabledWithPressurePlate || isDisabledByCorpse;
}
