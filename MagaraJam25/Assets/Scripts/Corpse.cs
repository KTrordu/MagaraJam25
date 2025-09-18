using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public static Corpse Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetCorpsePosition() => new Vector2(transform.position.x, transform.position.y);
}
