using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private Sprite leftSprite;

    private SpriteRenderer spriteRenderer;
    private Vector2 lookDirection;
    private bool isWalking;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerMoved += Player_OnPlayerMoved;
    }

    private void Player_OnPlayerMoved(object sender, System.EventArgs e)
    {
        isWalking = true;
    }

    private void Update()
    {
        lookDirection = Player.Instance.GetLookDirection();
        if (isWalking)
        {
            UpdateSpriteDirection(lookDirection);
        }
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                spriteRenderer.sprite = backSprite;
            }
            else
            {
                spriteRenderer.sprite = frontSprite;
            }
        }
        else
        {
            if (direction.x > 0)
            {
                spriteRenderer.sprite = rightSprite;
            }
            else
            {
                spriteRenderer.sprite = leftSprite;
            }
        }
        isWalking = false;
    }
}
