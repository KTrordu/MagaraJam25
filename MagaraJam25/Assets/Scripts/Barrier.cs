using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public enum BlockedEntities
    {
        Player,
        Corpse,
        Both
    }

    [SerializeField] private BlockedEntities blockedEntities;

    Collider2D barrierCollider;

    private void Awake()
    {
        barrierCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        HandleBlockedEntities();
    }

    private void HandleBlockedEntities()
    {
        switch (blockedEntities)
        {
            case BlockedEntities.Player:
                Physics2D.IgnoreCollision(barrierCollider, Corpse.Instance.GetCorpseCollider());
                break;

            case BlockedEntities.Corpse:
                Physics2D.IgnoreCollision(barrierCollider, Player.Instance.GetPlayerCollider());
                break;

            case BlockedEntities.Both:
                break;
        }
    }
}
