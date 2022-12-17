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

    // Get a reference to the player's Rigidbody2D component.
    Rigidbody2D rigidbody;

    public void Awake()
    {
        inputActions = new PlayerInputActions();
        rigidbody = GetComponent<Rigidbody2D>();
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
        // Update the player's velocity based on the input value and movement speed.
        rigidbody.velocity = direction * speed;
    }
}
