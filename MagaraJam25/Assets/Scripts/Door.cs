using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private PressurePlate pressurePlate;

    void Start()
    {
        pressurePlate.OnPressurePlatePressed += PressurePlate_OnPressurePlatePressed;
        pressurePlate.OnPressurePlateReleased += PressurePlate_OnPressurePlateReleased;
    }

    private void PressurePlate_OnPressurePlateReleased(object sender, System.EventArgs e)
    {
        CloseDoor();
    }

    private void PressurePlate_OnPressurePlatePressed(object sender, System.EventArgs e)
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        transform.Rotate(0, 0, 90f);
    }

    private void CloseDoor()
    {
        transform.Rotate(0, 0, -90f);
    }
}
