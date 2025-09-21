using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVisual : MonoBehaviour
{
    [SerializeField] private Sprite openFrontDoorSprite;
    [SerializeField] private Sprite closedFrontDoorSprite;
    [SerializeField] private Sprite openSideDoorSprite;
    [SerializeField] private Sprite closedSideDoorSprite;
    [SerializeField] private Door door;
    [SerializeField] private bool isFrontDoor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        door.OnDoorOpened += Door_OnDoorOpened;
        door.OnDoorClosed += Door_OnDoorClosed;
    }

    private void Door_OnDoorClosed(object sender, System.EventArgs e)
    {
        if (isFrontDoor)
        {
            spriteRenderer.sprite = closedFrontDoorSprite;
        }
        else
        {
            spriteRenderer.sprite = closedSideDoorSprite;
        }
    }

    private void Door_OnDoorOpened(object sender, System.EventArgs e)
    {
        if (isFrontDoor)
        {
            spriteRenderer.sprite = openFrontDoorSprite;
        }
        else
        {
            spriteRenderer.sprite = openSideDoorSprite;
        }
    }
}
