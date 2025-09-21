using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateVisual : MonoBehaviour
{
    [SerializeField] Transform pressurePlateTransform;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerMoved += Player_OnPlayerMoved;
        Corpse.Instance.OnCorpseThrown += Corpse_OnCorpseThrown;
        Corpse.Instance.OnCorpseSpiritPushed += Corpse_OnCorpseSpiritPushed;
    }

    private void Corpse_OnCorpseSpiritPushed(object sender, System.EventArgs e)
    {
        HandleVisual();
    }

    private void Corpse_OnCorpseThrown(object sender, System.EventArgs e)
    {
        HandleVisual();
    }

    private void Player_OnPlayerMoved(object sender, System.EventArgs e)
    {
        HandleVisual();
    }

    private void HandleVisual()
    {
        Vector2 pressurePlateLocalPosition = new Vector2(pressurePlateTransform.localPosition.x, pressurePlateTransform.localPosition.y);
        Vector2 playerPosition = Player.Instance.GetPlayerPosition();
        Vector2 corpsePosition = Corpse.Instance.GetCorpsePosition();
        if (pressurePlateLocalPosition == playerPosition || pressurePlateLocalPosition == corpsePosition)
        {
            spriteRenderer.sprite = pressedSprite;
        }
        else spriteRenderer.sprite = idleSprite;
    }
}
