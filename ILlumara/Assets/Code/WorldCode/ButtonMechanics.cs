using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonMechanics : MonoBehaviour
{

    [Header("Raycast Settings")]
    public RaycastHit2D ray;

    public float ray_Distens;
    public Vector2 ray_Offset;

    public GameObject niggatron;
    
    public bool ray_Found_Box;

    private void Awake()
    {
        ray_Found_Box = false;
        niggatron.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Raycast_Beam();
        Raycast_Cold_Detector();
        Debug.Log($"DEBUG : Is Box Found = {ray_Found_Box}");


        if (ray_Found_Box)
            niggatron.SetActive(true);
    }

    public void Raycast_Beam()
    {
        ray = Physics2D.Raycast(transform.position + (Vector3)ray_Offset, Vector2.up, ray_Distens);
    }

    public void Raycast_Cold_Detector()
    {
        if (ray.collider != null && ray.collider.CompareTag("Box"))
            ray_Found_Box = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPoint1 = transform.position + (Vector3)ray_Offset;
        Vector3 endPoint1 = startPoint1 + Vector3.up * ray_Distens;

        Gizmos.DrawLine(startPoint1, endPoint1);
    }

}
