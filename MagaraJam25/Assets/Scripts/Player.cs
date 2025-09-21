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
    public event EventHandler OnPlayerMoved;

    [SerializeField] private float corpsePickupRange = 1.0f;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float raycastOffset = 0.6f;
    [SerializeField] private float movementCooldownMax = 0.3f;
    [SerializeField] private float moveTileSpeed = 1.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform corpsePickUpPositionTransform;
    [SerializeField] private Transform corpseDropPositionTransform;

    private float lastMoveTime = -1f;
    private bool canMove = true;
    
    private bool isWalking = false;
    private bool isCarrying = false;
    private bool isUsedSpiritPush = false;

    private float movementCooldown;

    private Vector3 lookDirection;
    private Collider2D playerCollider;

    private void Awake()
    {
        Instance = this;

        lookDirection = Vector2.right;
        playerCollider = GetComponent<Collider2D>();

        movementCooldown = movementCooldownMax;
    }

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        GameInput.Instance.OnInteractAlternate += GameInput_OnInteractAlternate;
        GameInput.Instance.OnInteractAlternateHold_performed += GameInput_OnInteractAlternateHold_performed;
        GameInput.Instance.OnInteractAlternateHold_canceled += GameInput_OnInteractAlternateHold_canceled;

        RoomExit.OnRoomExitTriggered += RoomExit_OnRoomExitTriggered;
    }

    private void Update()
    {
        if (!Corpse.Instance.ShieldCorpse())
        {
            HandleMovement();            
        }
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

    private void HandleMovement()
    {
        if (Time.time - lastMoveTime < movementCooldown)
        {
            canMove = false;
            return;
        }
        else canMove = true;

        Vector2 moveDirection = gameInput.GetMovementVector();

        if (moveDirection != Vector2.zero && canMove)
        {
            lookDirection = moveDirection;

            Vector3 newPosition = transform.position;
            if (IsBlockedWithTrap(moveDirection)) return;
            if (IsBlockedWithWall(moveDirection)) return;

            if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x))
            {
                if (moveDirection.y > 0f)
                    newPosition += Vector3.up * moveTileSpeed;
                else if (moveDirection.y < 0f)
                    newPosition += Vector3.down * moveTileSpeed;
            }
            else
            {
                if (moveDirection.x > 0f)
                    newPosition += Vector3.right * moveTileSpeed;
                else if (moveDirection.x < 0f)
                    newPosition += Vector3.left * moveTileSpeed;
            }

            transform.position = newPosition;
            OnPlayerMoved?.Invoke(this, EventArgs.Empty);
            lastMoveTime = Time.time;
        }
    }

    private bool IsBlockedWithTrap(Vector2 moveDirection)
    {
        float offsetX;
        if (lookDirection.x > 0) offsetX = raycastOffset;
        else if (lookDirection.x < 0) offsetX = -raycastOffset;
        else offsetX = 0;

        float offsetY;
        if (lookDirection.y > 0) offsetY = raycastOffset;
        else if (lookDirection.y < 0) offsetY = -raycastOffset;
        else offsetY = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), lookDirection, moveTileSpeed);
        if (raycastHit.collider?.gameObject.GetComponent<Trap>() != null)
        {
            Trap trap = raycastHit.collider?.gameObject.GetComponent<Trap>();
            if (trap.IsDisabled()) return false;
            else return true;
        }
        else return false;
    }

    private bool IsBlockedWithWall(Vector2 moveDirection)
    {
        float offsetX;
        if (lookDirection.x > 0) offsetX = raycastOffset;
        else if (lookDirection.x < 0) offsetX = -raycastOffset;
        else offsetX = 0;

        float offsetY;
        if (lookDirection.y > 0) offsetY = raycastOffset;
        else if (lookDirection.y < 0) offsetY = -raycastOffset;
        else offsetY = 0;

        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), lookDirection, moveTileSpeed);
        if (raycastHit.collider?.gameObject.GetComponent<Wall>() != null) return true;
        else return false;
    }

    private void HandleInteract()
    {
        if (!isCarrying)
        {
            if (new Vector2(transform.position.x, transform.position.y) == Corpse.Instance.GetCorpsePosition())
            {
                PickCorpseUp();
                return;
            }

            if ((GetPlayerPosition() - Corpse.Instance.GetCorpsePosition()).magnitude >= corpsePickupRange) return;

            float offsetX;
            if (lookDirection.x > 0) offsetX = raycastOffset;
            else if (lookDirection.x < 0) offsetX = -raycastOffset;
            else offsetX = 0;

            float offsetY;
            if (lookDirection.y > 0) offsetY = raycastOffset;
            else if (lookDirection.y < 0) offsetY = -raycastOffset;
            else offsetY = 0;

            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), lookDirection, corpsePickupRange);
            if (raycastHit.collider?.gameObject.GetComponent<Corpse>() == null) return;

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
            
            Corpse.Instance.SpiritPushCorpse(new Vector2(lookDirection.x, lookDirection.y));
            isUsedSpiritPush = true;
        }

        if (isCarrying)
        {
            DropCorpse();
            Corpse.Instance.ThrowCorpse(new Vector2(lookDirection.x, lookDirection.y));
        }
    }

    private void HandleInteractAlternateHoldPerformed()
    {
        if (!isCarrying) return;
        Corpse.Instance.ShieldCorpse(true);
    }
    private void HandleInteractAlternateHoldCanceled()
    {
        if (!isCarrying) return;
        Corpse.Instance.ShieldCorpse(false);
    }
    
    private void PickCorpseUp()
    {
        Corpse.Instance.SetParent(transform);
        Corpse.Instance.SetLocalPosition(corpsePickUpPositionTransform.localPosition);
        isCarrying = !isCarrying;

        OnPlayerPickedUpCorpse?.Invoke(this, EventArgs.Empty);
    }

    private void DropCorpse()
    {
        Corpse.Instance.SetParent(null);
        Corpse.Instance.transform.position = transform.position + lookDirection;
        isCarrying = !isCarrying;

        OnPlayerDroppedCorpse?.Invoke(this, EventArgs.Empty);
    }

    public bool IsCarrying() => isCarrying;

    public Vector2 GetPlayerPosition() => new Vector2(transform.position.x, transform.position.y);

    public Collider2D GetPlayerCollider() => playerCollider;

    public Vector2 GetLookDirection() => new Vector2(lookDirection.x, lookDirection.y);
}