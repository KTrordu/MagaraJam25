using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float movementSpeedMax = 8f;
    [SerializeField] private float corpsePickupRange = 1.5f;
    [SerializeField] private float corpseSlowingFactor = 2f;
    [SerializeField] private GameInput gameInput;

    private bool isRotated = false;
    private bool isWalking = false;
    private bool isCarrying = false;

    private float movementSpeed;

    private void Awake()
    {
        Instance = this;

        movementSpeed = movementSpeedMax;
    }

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
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
            Corpse.Instance.transform.parent = transform;
            movementSpeed /= corpseSlowingFactor;
            isCarrying = !isCarrying;
        }
        else if (isCarrying)
        {
            Corpse.Instance.transform.parent = null;
            movementSpeed = movementSpeedMax;
            isCarrying = !isCarrying;
        }
    }

    public bool IsWalking() => isWalking;

    public Vector2 GetPlayerPosition() => new Vector2(transform.position.x, transform.position.y);
}