using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Player Movement Properties")]
    public float walkingSpeed;          // Speed at which the player walks
    public float jumpingForce;          // Force applied when the player jumps
    public float swayMovement;          // Smoothing factor for movement transitions

    [Header("Raycast Settings")]
    [Range(0, 1)]
    public float rayLength = 0.1f;      // Length of the raycast to detect the ground
    public Vector2 raycastOffset = new Vector2(0, -1);  // Offset for the raycast origin
    public bool raycastDebugger;        // Toggle for visualizing raycast in the editor

    private bool jumpableSurface;       // Checks if the player is on a surface they can jump from
    private bool jumpingInput;          // Checks if the jump input is being pressed
    private float movementInput;        // Stores the horizontal input for movement

    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {


        // Manage player inputs and movement each frame
        InputManager();
        MovementManager();
    }

    public void InputManager()
    {
        // Capture horizontal movement and jump input
        movementInput = Input.GetAxisRaw("Horizontal");
        jumpingInput = Input.GetKey(KeyCode.Space);
    }

    public void MovementManager()
    {
        // Handle player movement and jumping mechanics
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, movementInput * walkingSpeed, Time.deltaTime * swayMovement), rb.velocity.y);

        if (jumpingInput)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)raycastOffset, Vector2.down, rayLength);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))
                jumpableSurface = true;
        }

        if (jumpingInput && jumpableSurface)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingForce);
            jumpableSurface = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw debugging gizmos in the editor if enabled
        if (raycastDebugger)
            RaycastDebug();
    }

    private void RaycastDebug()
    {
        // Visualize the raycast used for detecting the ground
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position + (Vector3)raycastOffset;
        Vector3 endPoint = startPoint + Vector3.down * rayLength;
        Gizmos.DrawLine(startPoint, endPoint);

        // Change color if the player is on a jumpable surface
        if (jumpableSurface)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine(startPoint, endPoint);
    }
}