using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    // Declare variables to hold references to the player's actions and the egg's transform
    PlayerInputActions inputActions;
    // A reference to the custom cursor texture
    public Texture2D cursorTexture;

    void Awake()
    {
        // Set the custom cursor texture
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        // Set up the input actions
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Enable the input actions and bind the actions to the appropriate methods
        inputActions.Enable();
        // Bind the OnClick action to the OnMouseDown method
        inputActions.Player.Click.performed += OnMouseDown;

        // Bind the OnDrag action to the OnMouseDrag method
        inputActions.Player.Release.performed += OnMouseUp;
    }

    void OnDisable()
    {
        // Unbind the OnClick action from the OnMouseDown method
        inputActions.Player.Click.performed -= OnMouseDown;

        // Unbind the OnDrag action from the OnMouseDrag method
        inputActions.Player.Release.performed -= OnMouseUp;
        inputActions.Disable();
    }

    void OnDestroy()
    {
        // Dispose of the input actions when they are no longer needed
        inputActions.Dispose();
    }

    private void OnMouseDown(InputAction.CallbackContext context)
    {
        held = true;
        // Raycast from the mouse position to find a creature
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        // If the raycast hit a creature
        if (hit.collider != null)
        {
            Debug.Log("HIT!");
            Creature creature = hit.collider.gameObject.GetComponent<Creature>();
            heldcreature = creature;
            // Update the time at which the creature received attention
            creature.SetLastAttentionTime();
            // Get a reference to the creature's hatching controller
            HatchingController hatchingController = creature.hatchingController;
            creature.Click();
            // If the creature is in its egg stage
            if (hatchingController != null && !hatchingController.isHatched)
            {
                // Hasten the hatching process
                hatchingController.HastenHatching();
            }

            // Set the isDragged flag to true
            creature.movementController.isDragged = true;
        }
    }
    Creature heldcreature = null;
    private void OnDrag()
    {
        if (heldcreature == null) return;
        Debug.Log("HIT2!");
        // Update the time at which the creature received attention
        heldcreature.SetLastAttentionTime();

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Set the creature's position to the mouse position
        heldcreature.gameObject.transform.position = mousePosition;

        // Calculate the speed at which the creature was dragged
        float dragSpeed = (mousePosition - heldcreature.gameObject.transform.position).magnitude / Time.deltaTime;

        //// If the drag speed is above a certain threshold, make the creature feel sad
        //if (dragSpeed > 5.0f)
        //{
        //    heldcreature.ChangeHappiness(-10);
        //    heldcreature.Speak(CreatureLanguage.Emotion.Sadness);
        //}
        //// Otherwise, make the creature feel happy
        //else
        //{
        //    heldcreature.ChangeHappiness(10);
        //    heldcreature.Speak(CreatureLanguage.Emotion.Happiness);
        //}
    }

    bool held = false;

    private void OnMouseUp(InputAction.CallbackContext context)
    {
        held = false;
        heldcreature = null;
        // Raycast from the mouse position to find a creature
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        // If the raycast hit a creature
        if (hit.collider != null)
        {
            Debug.Log("HIT3!");
            Creature creature = hit.collider.gameObject.GetComponent<Creature>();
            // Set the isDragged flag to false
            creature.movementController.isDragged = false;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        if (held)
        {
            OnDrag();
        }
    }
}
