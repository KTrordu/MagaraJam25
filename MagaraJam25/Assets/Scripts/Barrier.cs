using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public enum BlockedEntities
    {
        Player,
        Corpse
    }

    [SerializeField] private BlockedEntities blockedEntities;

    public BlockedEntities GetBlockedEntities() { return blockedEntities; }

    public bool IsBlockingPlayer() => blockedEntities == BlockedEntities.Player;
    public bool IsBlockingCorpse() => blockedEntities == BlockedEntities.Corpse;
}
