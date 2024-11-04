using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public RaycastHit2D ray_L;
    public RaycastHit2D ray_R;

    public Vector2 ray_Offset_L;
    public Vector2 ray_Offset_R;

    public bool Pass;

    private bool raycast_Hit_L;
    private bool raycast_Hit_R;

    private bool player_on_Side_L;
    private bool player_on_Side_R; 

    public float ray_Distens;
    [HideInInspector] public float movement;

    private void Awake()
    {
        raycast_Hit_L = false;
        raycast_Hit_R = false;

        player_on_Side_L = false;
        player_on_Side_R = false;
    }

    // Update is called once per frame
    void Update()
    {
        Raycast_ray();
        Raycast_Check();
        Player_Placement();
    }

    void Raycast_ray()
    {
        ray_L = Physics2D.Raycast(transform.position + (Vector3)ray_Offset_L, Vector2.left, ray_Distens);
        ray_R = Physics2D.Raycast(transform.position + (Vector3)ray_Offset_R, Vector2.right, ray_Distens);
    }

    void Raycast_Check()
    {
        Pass = false;

        if (ray_L.collider != null && ray_L.collider.CompareTag("Player"))
            raycast_Hit_L = true;

        if (ray_R != null && ray_R.collider != null && ray_R.collider.CompareTag("Player"))
            raycast_Hit_R = true;
    }

    void Player_Placement()
    {
        if (raycast_Hit_L)
            player_on_Side_L = true;
        if (raycast_Hit_L && raycast_Hit_R && !player_on_Side_L)
        {
            movement = 1;
            player_on_Side_L = false;
            raycast_Hit_R = false;
            Pass = true;
            Debug.Log($"DEBUG : MOVE CAM LEFT");
        }

        if (raycast_Hit_R)
            player_on_Side_R = true;
        if (player_on_Side_R && raycast_Hit_L && !raycast_Hit_R)
        {
            movement = -1;
            player_on_Side_L = false;
            raycast_Hit_L = false;
            Pass = true;
            Debug.Log($"DEBUG : MOVE CAM RIGHT");

        }

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
