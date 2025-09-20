using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Corpse : RoomTransitable
{
    public static Corpse Instance { get; private set; }

    [SerializeField] private float corpseThrowTimeMax = 0.5f;
    [SerializeField] private float corpseThrowVelocity = 10f;
    [SerializeField] private float corpseSpiritPushVelocity = 5f;

    private Rigidbody2D rigidBody;
    private Collider2D corpseCollider;
    private float corpseThrowTime;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();
        corpseCollider = GetComponent<Collider2D>();

        corpseThrowTime = 0;
    }

    private void Update()
    {
        HandleThrowTimer();
    }

    private void HandleThrowTimer()
    {
        if (corpseThrowTime > 0) corpseThrowTime -= Time.deltaTime;
        else if (corpseThrowTime <= 0)
        {
            StopCorpse();
            corpseThrowTime = 0f;
        }
    }

    public Vector2 GetCorpsePosition() => new Vector2(transform.position.x, transform.position.y);

    public void ThrowCorpse(Vector3 throwDirection)
    {
        rigidBody.velocity = new Vector2(throwDirection.x * corpseThrowVelocity, throwDirection.y * corpseThrowVelocity);
        corpseThrowTime = corpseThrowTimeMax;
    }

    public void SpiritPushCorpse(Vector3 throwDirection)
    {
        rigidBody.velocity = new Vector2(throwDirection.x * corpseSpiritPushVelocity, throwDirection.y * corpseSpiritPushVelocity);
        corpseThrowTime = corpseThrowTimeMax;
    }

    public void StopCorpse()
    {
        rigidBody.velocity = Vector3.zero;
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
    public void ShieldCorpse()
    {

    }

    public void StopShieldingCorpse()
    {

    }
}
