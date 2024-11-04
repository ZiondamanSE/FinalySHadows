using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbing : MonoBehaviour
{
    PlayerMovementScript pms;

    [SerializeField] public GameObject Center;
    [HideInInspector] public bool grabbing;

    public float distance = 2f;  // Fixed distance from Point A

    // Clamping angle range for X and Y axes, adjustable in the Inspector
    [SerializeField] private float minClampAngle = -90f;
    [SerializeField] private float maxClampAngle = 90f;

    // Toggle for enabling/disabling the cone visualization
    [SerializeField] private bool showCone = true;

    void Awake()
    {
        if (pms == null)
            pms = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        ObjectMovement();


        if (showCone)
        {
            ShowMovementCone();
        }
    }

    private void ObjectMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;  // Set z to 0 because we're in 2D

        Vector3 direction = mousePosition - Center.transform.position;
        direction.Normalize();  // Normalize to get a unit vector

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, minClampAngle, maxClampAngle);

        float radianAngle = angle * Mathf.Deg2Rad;
        Vector3 clampedDirection = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0f);

        transform.position = Center.transform.position + clampedDirection * distance;
    }

    private void ShowMovementCone() // Dont ask me how this works i got gods hands working in the background.
    {
        // Calculate the direction for the min and max clamped angles
        float minRadianAngle = minClampAngle * Mathf.Deg2Rad;
        float maxRadianAngle = maxClampAngle * Mathf.Deg2Rad;

        Vector3 minDirection = new Vector3(Mathf.Cos(minRadianAngle), Mathf.Sin(minRadianAngle), 0f) * distance;
        Vector3 maxDirection = new Vector3(Mathf.Cos(maxRadianAngle), Mathf.Sin(maxRadianAngle), 0f) * distance;

        // Draw rays representing the cone boundary
        Debug.DrawRay(Center.transform.position, minDirection, Color.red);
        Debug.DrawRay(Center.transform.position, maxDirection, Color.red);
    }
}