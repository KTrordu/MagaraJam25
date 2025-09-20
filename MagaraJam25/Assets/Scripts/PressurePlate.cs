using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public event EventHandler OnPressurePlatePressed;
    public event EventHandler OnPressurePlateReleased;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null && collision.gameObject.GetComponent<Corpse>() == null) return;
        OnPressurePlatePressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPressurePlateReleased?.Invoke(this, EventArgs.Empty);
    }
}
