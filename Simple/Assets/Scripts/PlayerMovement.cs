using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // This variable will store the player's movement speed.
    public float speed = 5.0f;

    // Get a reference to the player's input actions.
    PlayerInputActions inputActions;

    // This variable will store the input values.
    Vector2 direction;

    public void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    // Subscribe to the input actions events.
    void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerControls.Movement.performed += Movement_performed;
        inputActions.PlayerControls.Movement.canceled += Movement_canceled;
        Debug.Log("Enabled");
    }

    // Unsubscribe from the input actions events.
    void OnDisable()
    {
        inputActions.Disable();
        inputActions.PlayerControls.Movement.performed -= Movement_performed;
        inputActions.PlayerControls.Movement.canceled -= Movement_canceled;
        Debug.Log("Disabled");
    }

    // This function will be called when the movement input is performed.
    void Movement_performed(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        Debug.Log(direction);
    }

    // This function will be called when the movement input is canceled.
    void Movement_canceled(InputAction.CallbackContext context)
    {
        direction = Vector2.zero;
    }

    // Update is called once per frame.
    void Update()
    {
        // Normalize the direction vector to avoid diagonal movement being faster.
        direction = direction.normalized;
        // Apply the movement to the player's position.
        transform.position = transform.position + (Vector3)direction * speed * Time.deltaTime;
    }
}