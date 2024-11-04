using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPhysics : MonoBehaviour
{
    Rigidbody2D rb; // Gravity Control and Raycast

    [Header("Importing Scripts")]
    public PlayerMovementScript pms_Script; // User Input
    public ButtonMechanics bm_Script;

    [Header("Box Mobility")]
    public GameObject button_Point;
    public GameObject interactive_Point;
    public float speed;

    [HideInInspector] public bool when_Grabbable; // Allow Holding as Long as It Is Not Let Go

    [Header("Raycast Settings")]
    public float ray_Distens;

    // Allow More Control on Distances and Offset
    public Vector2 ray_Offset_L;
    public Vector2 ray_Offset_R;

    // Raycast Beam for Each Respective Direction
    public RaycastHit2D ray_L;
    public RaycastHit2D ray_R;

    public bool ray_Found_Player;

    private void Awake()
    {
        // If the Import of pms_Scripts FailedS
        if (pms_Script == null)
            pms_Script = GetComponent<PlayerMovementScript>();
        if (pms_Script == null)
            Debug.LogError("PlayerMovementScript is not assigned or found!");
        if(bm_Script == null)
            bm_Script = GetComponent<ButtonMechanics>();
        if (bm_Script == null)
            Debug.LogError("ButtonMechanics is not assigned or found!");

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (bm_Script.ray_Found_Box != true)
        {
            Raycast_Beam();
            Raycast_Cold_Detector();

            if (ray_Found_Player && pms_Script.pickupInput)
                Box_Moving_Physics();
            if (when_Grabbable && pms_Script.pickupInput)
                Box_Moving_Physics();
            if (!pms_Script.pickupInput)
            {
                when_Grabbable = false;
                rb.gravityScale = 1;
            }
        }
        else if (bm_Script.ray_Found_Box == true)
        {
            transform.position = button_Point.transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
            rb.gravityScale = 0;
        }
    }

    public void Raycast_Beam()
    {
        ray_L = Physics2D.Raycast(transform.position + (Vector3)ray_Offset_L, Vector2.left, ray_Distens);
        ray_R = Physics2D.Raycast(transform.position + (Vector3)ray_Offset_R, Vector2.right, ray_Distens);
    }

    public void Raycast_Cold_Detector()
    {
        ray_Found_Player = false; // Reset before checking

        if (ray_L.collider != null && ray_L.collider.CompareTag("Player"))
            ray_Found_Player = true;
        else if (ray_R.collider != null && ray_R.collider.CompareTag("Player"))
            ray_Found_Player = true;
    }

    void Box_Moving_Physics()
    {
        transform.position = new Vector2(Mathf.Lerp(transform.position.x, interactive_Point.transform.position.x, Time.deltaTime * speed), Mathf.Lerp(transform.position.y, interactive_Point.transform.position.y, Time.deltaTime * speed));
        rb.gravityScale = 0;
        ray_Found_Player = false;
        when_Grabbable = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPoint1 = transform.position + (Vector3)ray_Offset_L;
        Vector3 endPoint1 = startPoint1 + Vector3.left * ray_Distens;

        Vector3 startPoint2 = transform.position + (Vector3)ray_Offset_R;
        Vector3 endPoint2 = startPoint2 + Vector3.right * ray_Distens;

        Gizmos.DrawLine(startPoint1, endPoint1);
        Gizmos.DrawLine(startPoint2, endPoint2);
    }
}