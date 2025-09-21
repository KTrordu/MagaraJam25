using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;
    public event EventHandler<OnResetEventArgs> OnReset;
    public class OnResetEventArgs : EventArgs
    {
        public Room currentRoom;
    }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Reset.performed += Reset_performed;
        playerInputActions.Player.Enable();
    }

    private void Reset_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReset?.Invoke(this, new OnResetEventArgs
        {
            currentRoom = Player.Instance.currentRoom
        });
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputActions.Dispose();
    }

    public Vector2 GetMovementVector()
    {
        return playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLookingVector()
    {
        return playerInputActions.Player.Looking.ReadValue<Vector2>();
    }
}