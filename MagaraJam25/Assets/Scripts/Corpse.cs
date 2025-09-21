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
    [SerializeField] private float raycastOffset = 0.6f;

    private Rigidbody2D rigidBody;
    private Collider2D corpseCollider;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();
        corpseCollider = GetComponent<Collider2D>();
    }

    public Vector2 GetCorpsePosition() => new Vector2(transform.position.x, transform.position.y);

    public bool ThrowCorpse(Vector2 throwingVector)
    {
        float offsetX;
        if (throwingVector.x > 0) offsetX = raycastOffset;
        else if (throwingVector.x < 0) offsetX = -raycastOffset;
        else offsetX = 0;

        float offsetY;
        if (throwingVector.y > 0) offsetY = raycastOffset;
        else if (throwingVector.y < 0) offsetY = -raycastOffset;
        else offsetY = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), throwingVector, moveTileSpeed);
        if (raycastHit.collider?.gameObject.GetComponent<Wall>() != null) return false;

        transform.position += new Vector3(throwingVector.x * moveTileSpeed, throwingVector.y * moveTileSpeed, 0f);
        OnCorpseThrown?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public bool SpiritPushCorpse(Vector2 throwingVector)
    {
        float offsetX;
        if (throwingVector.x > 0) offsetX = raycastOffset;
        else if (throwingVector.x < 0) offsetX = -raycastOffset;
        else offsetX = 0;

        float offsetY;
        if (throwingVector.y > 0) offsetY = raycastOffset;
        else if (throwingVector.y < 0) offsetY = -raycastOffset;
        else offsetY = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), throwingVector, moveTileSpeed);
        if (raycastHit.collider?.gameObject.GetComponent<Wall>() != null) return false;

        transform.position += new Vector3(throwingVector.x * moveTileSpeed, throwingVector.y * moveTileSpeed, 0f);
        OnCorpseSpiritPushed?.Invoke(this, EventArgs.Empty);
        return true;
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
}
