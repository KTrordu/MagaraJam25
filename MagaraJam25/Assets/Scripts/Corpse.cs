using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Corpse : RoomTransitable
{
    public static Corpse Instance { get; private set; }

    public event EventHandler OnCorpseThrown;
    public event EventHandler OnCorpseSpiritPushed;

    [SerializeField] private float moveTileSpeed = 1.0f;

    private Rigidbody2D rigidBody;
    private Collider2D corpseCollider;

    private bool isShielding = false;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();
        corpseCollider = GetComponent<Collider2D>();
    }

    public Vector2 GetCorpsePosition() => new Vector2(transform.position.x, transform.position.y);

    public void ThrowCorpse(Vector2 throwingVector)
    {
        transform.position += new Vector3(throwingVector.x * moveTileSpeed, throwingVector.y * moveTileSpeed, 0f);
        OnCorpseThrown?.Invoke(this, EventArgs.Empty);
    }

    public void SpiritPushCorpse(Vector2 throwingVector)
    {
        transform.position += new Vector3(throwingVector.x * moveTileSpeed, throwingVector.y * moveTileSpeed, 0f);
        OnCorpseSpiritPushed?.Invoke(this, EventArgs.Empty);
    }

    public Collider2D GetCorpseCollider() => corpseCollider;

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }

    public void SetLocalPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    public void ShieldCorpse(bool isShielding)
    {
        this.isShielding = isShielding;
    }

    public bool ShieldCorpse() => isShielding;
}
