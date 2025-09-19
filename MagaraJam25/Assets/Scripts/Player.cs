using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : RoomTransitable
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerPickedUpCorpse;
    public event EventHandler OnPlayerDroppedCorpse;

    [SerializeField] private float movementSpeedMax = 8f;
    [SerializeField] private float corpsePickupRange = 1.5f;
    [SerializeField] private float corpseSlowingFactor = 2f;
    [SerializeField] private float raycastOffset = 0.6f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform corpsePositionTransform;

    private bool isRotated = false;
    private bool isWalking = false;
    private bool isCarrying = false;
    private bool isUsedSpiritPush = false;

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
        GameInput.Instance.OnInteractAlternateHold_performed += GameInput_OnInteractAlternateHold_performed;
        GameInput.Instance.OnInteractAlternateHold_canceled += GameInput_OnInteractAlternateHold_canceled;

        RoomExit.OnRoomExitTriggered += RoomExit_OnRoomExitTriggered;
    }

    private void RoomExit_OnRoomExitTriggered(object sender, EventArgs e)
    {
        isUsedSpiritPush = false;
    }

    private void GameInput_OnInteractAlternateHold_canceled(object sender, EventArgs e)
    {
        HandleInteractAlternateHoldCanceled();
    }
    private void GameInput_OnInteractAlternateHold_performed(object sender, EventArgs e)
    {
        HandleInteractAlternateHoldPerformed();
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
        if (!isCarrying && !isUsedSpiritPush)
        {
            float offsetX;
            if (lookDirection.x > 0) offsetX = raycastOffset;
            else if (lookDirection.x < 0) offsetX = -raycastOffset;
            else offsetX = 0;

            float offsetY;
            if (lookDirection.y > 0) offsetY = raycastOffset;
            else if (lookDirection.y < 0) offsetY = -raycastOffset;
            else offsetY = 0;

            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), lookDirection);
            if (raycastHit.collider?.gameObject.GetComponent<Corpse>() == null) return;
            
            Corpse.Instance.ThrowCorpse(lookDirection);
            isUsedSpiritPush = true;
        }

        if (isCarrying)
        {
            DropCorpse();
            Corpse.Instance.ThrowCorpse(lookDirection);
        }
    }

    private void HandleInteractAlternateHoldPerformed()
    {
        if (!isCarrying) return;
        Corpse.Instance.ShieldCorpse();
    }
    private void HandleInteractAlternateHoldCanceled()
    {
        if (!isCarrying) return;
        Corpse.Instance.StopShieldingCorpse();
    }

    
    private void PickCorpseUp()
    {
        Corpse.Instance.SetParent(transform);
        Corpse.Instance.SetLocalPosition(corpsePositionTransform.localPosition);
        movementSpeed /= corpseSlowingFactor;
        isCarrying = !isCarrying;
        Corpse.Instance.StopCorpse();

        OnPlayerPickedUpCorpse?.Invoke(this, EventArgs.Empty);
    }

    private void DropCorpse()
    {
        Corpse.Instance.SetParent(null);
        movementSpeed = movementSpeedMax;
        isCarrying = !isCarrying;

        OnPlayerDroppedCorpse?.Invoke(this, EventArgs.Empty);
    }

    public bool IsWalking() => isWalking;

    public bool IsCarrying() => isCarrying;

    public Vector2 GetPlayerPosition() => new Vector2(transform.position.x, transform.position.y);

    public Collider2D GetPlayerCollider() => playerCollider;
}