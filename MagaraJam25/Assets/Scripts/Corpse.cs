using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Corpse : MonoBehaviour
{
    public static Corpse Instance;

    [SerializeField] private float corpseThrowTimeMax = 0.5f;
    [SerializeField] private float corpseThrowVelocity = 30f;

    private Rigidbody2D rigidBody;
    private float corpseThrowTime;

    private void Awake()
    {
        Instance = this;

        rigidBody = GetComponent<Rigidbody2D>();

        corpseThrowTime = 0;
    }

    private void Update()
    {
        HandleThrowTimer();
    }

    public Vector2 GetCorpsePosition() => new Vector2(transform.position.x, transform.position.y);

    public void ThrowCorpse(Vector3 throwDirection)
    {
        rigidBody.velocity = new Vector2(throwDirection.x * corpseThrowVelocity, throwDirection.y * corpseThrowVelocity);
        corpseThrowTime = corpseThrowTimeMax;
    }

    public void StopCorpse()
    {
        rigidBody.velocity = Vector3.zero;
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
}
