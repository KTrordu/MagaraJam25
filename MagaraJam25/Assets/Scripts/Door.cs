using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event EventHandler OnDoorOpened;
    public event EventHandler OnDoorClosed;

    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private bool isPermanent;

    private bool isOpened;
    private Collider2D doorCollider;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        pressurePlate.OnPressurePlatePressed += PressurePlate_OnPressurePlatePressed;
        pressurePlate.OnPressurePlateReleased += PressurePlate_OnPressurePlateReleased;
    }

    private void PressurePlate_OnPressurePlateReleased(object sender, System.EventArgs e)
    {
        if (isPermanent) return;
        CloseDoor();
    }

    private void PressurePlate_OnPressurePlatePressed(object sender, System.EventArgs e)
    {
        if (isOpened) return;
        OpenDoor();
    }

    private void OpenDoor()
    {
        isOpened = true;
        doorCollider.enabled = false;
    }

    private void CloseDoor()
    {
        isOpened = false;
        doorCollider.enabled = true;
    }
}
