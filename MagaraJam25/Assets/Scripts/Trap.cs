using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public event EventHandler OnTrapPressedByPlayer;

    [SerializeField] private bool isDisabled = false;

    private Collider2D trapCollider;

    private bool isBlockedWithCorpse;

    private void Awake()
    {
        trapCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerPickedUpCorpse += Player_OnPlayerPickedUpCorpse;
        Player.Instance.OnPlayerDroppedCorpse += Player_OnPlayerDroppedCorpse;
    }

    private void Player_OnPlayerDroppedCorpse(object sender, EventArgs e)
    {
        if (trapCollider.IsTouching(Corpse.Instance.GetCorpseCollider()))
        {
            isBlockedWithCorpse = true;
            Debug.Log("blocked");
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
            isBlockedWithCorpse = true;
            Debug.Log("blocked");
        }

        if (collision.gameObject.GetComponent<Corpse>() != null && Player.Instance.IsCarrying())
        {
            HandleCorpseRemoval();
        }

        if (collision.gameObject.GetComponent<Player>() != null && !isBlockedWithCorpse)
        {
            OnTrapPressedByPlayer?.Invoke(this, EventArgs.Empty);
            Debug.Log("player trap");
        }
    }

    private void HandleCorpseRemoval()
    {
        if (isBlockedWithCorpse)
        {
            isBlockedWithCorpse = false;
            Debug.Log("unblocked");
        }
    }

    public bool IsDisabled() => isDisabled;
}
