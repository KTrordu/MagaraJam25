using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private GameInput gameInput;

    private bool isRotated = false;
    private bool isWalking = false;

    private void Awake()
    {
    }

    private void Start()
    {
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

    public bool IsWalking() => isWalking;
}