using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField] private Transform player_Pos;
    public Vector2 offset;
    public float movement_Speed;

    void Update()
    {
        Vector3 targetPosition = new Vector3(Mathf.Lerp(transform.position.x, player_Pos.position.x + offset.x, Time.deltaTime * movement_Speed), Mathf.Lerp(transform.position.y, player_Pos.position.y + offset.y, Time.deltaTime * movement_Speed), transform.position.z);
        transform.position = targetPosition;
    }
}
