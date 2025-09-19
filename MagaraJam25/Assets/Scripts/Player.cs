using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler OnPlayerPickedUpCorpse;
    public event EventHandler OnPlayerDroppedCorpse;

    [SerializeField] private float movementSpeedMax = 8f;
    [SerializeField] private float corpsePickupRange = 1.5f;
    [SerializeField] private float corpseSlowingFactor = 2f;
    [SerializeField] private GameInput gameInput;

    private bool isRotated = false;
    private bool isWalking = false;
    private bool isCarrying = false;

    private float movementSpeed;
    private Vector3 lookDirection;
    private Collider2D playerCollider;

    private void Awake()
    {
        Instance = this;

        movementSpeed = movementSpeedMax;
        lookDirection = Vector2.right;
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        GameInput.Instance.OnInteractAlternate += GameInput_OnInteractAlternate;
    }

    private void GameInput_OnInteractAlternate(object sender, EventArgs e)
    {
        HandleInteractAlternate();
    }

    private void GameInput_OnInteract(object sender, EventArgs e)
    {
        HandleInteract();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVector();

        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0f);
        if (moveDirection != Vector3.zero) lookDirection = moveDirection;

        if (moveDirection.magnitude > 0f)
        {
            transform.position += moveDirection * movementSpeed * Time.deltaTime;

            if (!isRotated && moveDirection.x < 0f)
            {
                transform.Rotate(0f, 180f, 0f);
                isRotated = !isRotated;
            }
            if (isRotated && moveDirection.x > 0f)
            {
                transform.Rotate(0f, 180f, 0f);
                isRotated = !isRotated;
            }

            isWalking = true;
        }
        else isWalking = false;
    }

    private void HandleInteract()
    {
        if ((GetPlayerPosition() - Corpse.Instance.GetCorpsePosition()).magnitude >= corpsePickupRange) return;

        if (!isCarrying)
        {
            PickCorpseUp();
        }
        else if (isCarrying)
        {
            DropCorpse();
        }
    }

    private void HandleInteractAlternate()
    {
        if (!isCarrying) return;

        DropCorpse();
        Corpse.Instance.ThrowCorpse(lookDirection);
    }
    
    private void PickCorpseUp()
    {
        Corpse.Instance.transform.parent = transform;
        movementSpeed /= corpseSlowingFactor;
        isCarrying = !isCarrying;
        Corpse.Instance.StopCorpse();

        OnPlayerPickedUpCorpse?.Invoke(this, EventArgs.Empty);
    }

    private void DropCorpse()
    {
        Corpse.Instance.transform.parent = null;
        movementSpeed = movementSpeedMax;
        isCarrying = !isCarrying;

        OnPlayerDroppedCorpse?.Invoke(this, EventArgs.Empty);
    }

    public bool IsWalking() => isWalking;

    public bool IsCarrying() => isCarrying;

    public Vector2 GetPlayerPosition() => new Vector2(transform.position.x, transform.position.y);

    public Collider2D GetPlayerCollider() => playerCollider;
}