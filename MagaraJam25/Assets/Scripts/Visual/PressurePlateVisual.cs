using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateVisual : MonoBehaviour
{
    [SerializeField] Transform pressurePlateTransform;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;
    private PressurePlate pressurePlate;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        pressurePlate = pressurePlateTransform.gameObject.GetComponent<PressurePlate>();
        pressurePlate.OnPressurePlatePressed += PressurePlate_OnPressurePlatePressed;
        pressurePlate.OnPressurePlateReleased += PressurePlate_OnPressurePlateReleased;
    }

    private void PressurePlate_OnPressurePlateReleased(object sender, System.EventArgs e)
    {
        DisableSprite();
    }

    private void PressurePlate_OnPressurePlatePressed(object sender, System.EventArgs e)
    {
        EnableSprite();
    }

    private void EnableSprite()
    {
        spriteRenderer.sprite = pressedSprite;
    }

    private void DisableSprite()
    {
        spriteRenderer.sprite = idleSprite;
    }
}
