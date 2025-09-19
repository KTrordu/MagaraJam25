using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public static event EventHandler OnRoomExitTriggered;

    [SerializeField] public Room roomToGo;

    private Collider2D roomExitCollider;

    private void Awake()
    {
        roomExitCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            OnRoomExitTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}
