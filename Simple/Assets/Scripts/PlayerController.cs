using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Declare variables to hold references to the player's actions and the egg's transform
    PlayerInputActions inputActions;
    public Transform eggTransform; // Make the egg transform public so it can be set in the Inspector
    public float throwForce = 100; // The magnitude of the force to apply to the egg when it is thrown

    void Awake()
    {
        // Set up the input actions
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Enable the input actions and bind the actions to the appropriate methods
        inputActions.Enable();
        inputActions.Player.Egg.performed += Egg;
    }

    void OnDisable()
    {
        // Disable the input actions and unbind the actions from the appropriate methods
        inputActions.Player.Egg.performed -= Egg;
        inputActions.Disable();
    }

    void OnDestroy()
    {
        // Dispose of the input actions when they are no longer needed
        inputActions.Dispose();
    }
    void Egg(InputAction.CallbackContext context)
    {
        // Pick up the egg if it is not already picked up
        if (eggTransform.parent == null)
        {
            eggTransform.SetParent(transform);
            // Set the egg's position to be in front of the player
            eggTransform.localPosition = new Vector3(1, 0, 0);

            // Make the egg follow the player's velocity by setting its Rigidbody2D component's velocity property to the player's velocity
            Rigidbody2D eggRigidbody = eggTransform.GetComponent<Rigidbody2D>();
            eggRigidbody.velocity = Vector2.zero;
        }
        // Throw the egg if it is picked up
        else if (eggTransform.parent == transform)
        {
            eggTransform.SetParent(null);
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // If the egg is picked up, update its position based on the player's velocity and set its angular velocity and velocity to 0 to prevent it from rotating or moving on its own
        if (eggTransform != null && eggTransform.parent == transform)
        {
            Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
            eggTransform.position += (Vector3)playerRigidbody.velocity * Time.deltaTime;

            Rigidbody2D eggRigidbody = eggTransform.GetComponent<Rigidbody2D>();
            eggRigidbody.angularVelocity = 0;
            eggRigidbody.velocity = Vector2.zero;
        }
    }
}
