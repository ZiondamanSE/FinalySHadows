using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] LevelTrans levelTrans;
    private Rigidbody2D rb;

    [Header("Player Movement Properties")]
    public float walkingSpeed;
    public float jumpingForce;
    public float swayMovement;

    [Header("Raycast Settings")]
    [Range(0, 1)]
    public float rayLength = 0.1f;
    public Vector2 raycastOffset = new Vector2(0, -1);
    public bool raycastDebugger;

    private HashSet<Collider2D> safeZones = new HashSet<Collider2D>(); // To track active safe zones
    private bool jumpableSurface;
    private bool jumpingInput;
    [HideInInspector] public bool pickupInput;
    private float movementInput;
    bool isInSafeZone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(levelTrans == null)
            levelTrans = GetComponent<LevelTrans>();
    }

    void FixedUpdate()
    {
        InputManager();
        MovementManager();
    }

    void Update()
    {
        if (isInSafeZone != true)
            HandleDeath();
    }

    void InputManager()
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        jumpingInput = Input.GetKey(KeyCode.Space);
        pickupInput = Input.GetKey(KeyCode.E);
    }

    void MovementManager()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, movementInput * walkingSpeed, Time.deltaTime * swayMovement), rb.velocity.y);

        if (jumpingInput)
        { // so when the gay ray hits the normal man he become puddel of bupe.
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Safe"))
        {
            safeZones.Add(collider);
            Debug.LogWarning("INFO : Entered Safe Zone");
            isInSafeZone = true;
        }
        else if (collider.CompareTag("non-touchable-object"))
        {
            if (safeZones.Count == 0)
            {
                HandleDeath();
            }
            else
            {
                Debug.LogWarning("INFO : Collided with non-touchable object but in safe zone");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Safe"))
        {
            safeZones.Remove(collider);
            Debug.LogWarning("INFO : Exited Safe Zone");

            // Check if all safe zones are exited
            if (safeZones.Count == 0)
            {
                Debug.LogWarning("INFO : Player is no longer in any safe zone.");
                isInSafeZone = false;
            }
        }
    }

    private void HandleDeath()
    {
        Debug.LogWarning("INFO : Player has died!");
        Debug.LogWarning("INFO : Calling Restart_Game");
        if (levelTrans != null)
        {
            levelTrans.StartCoroutine(levelTrans.Restart_Game());
        }
        else
        {
            Debug.LogError("ERROR: levelTrans reference is missing!");
        }
        gameObject.SetActive(false);
    }



    private void OnDrawGizmos()
    {
        if (raycastDebugger)
            RaycastDebug();
    }

    private void RaycastDebug()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position + (Vector3)raycastOffset;
        Vector3 endPoint = startPoint + Vector3.down * rayLength;
        Gizmos.DrawLine(startPoint, endPoint);

        if (jumpableSurface)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawLine(startPoint, endPoint);
    }
}
