using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class cameraMovment : MonoBehaviour
{
    [SerializeField] private triggerSystem ts; // Make sure this is assigned or present on the GameObject

    public Vector2 MaxCamMove;
    public float cam_m_s;
    public float stepping_x;
    private float goal;

    bool m_up;
    bool stepping_can;
    void Awake()
    {
        if (ts == null)
            ts = GetComponent<triggerSystem>();

        stepping_can = true;
    }

    void cameraSystem()
    {
        // Null check before accessing ts
        if (ts.cameraStepping == -1)
            m_up = false;
            //Debug.Log($"DEBUG : Camera is moving : {m_up}");


        else if (ts.cameraStepping == 1)
            m_up = true;
            //Debug.Log($"DEBUG : Camera is moving : {m_up}");
    }

    void Update()
    {
        if(ts.m_Cam == true)
        {
            cameraSystem();
            if (stepping_can == true)
                MovmentPos();
        }

        if (MaxCamMove.x > 0 || MaxCamMove.y > 0)
        {
            if (ts.m_Cam == false)
            {
                OnCall();
            }
        }
        else
            Debug.LogError($"ERROR : Out of movment limit : x = {MaxCamMove.x}, y = {MaxCamMove.y}.");
    }

    void OnCall()
    {
        // Lerp towards the goal
        float x = Mathf.Lerp(transform.position.x, goal, Time.deltaTime * cam_m_s);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);

        // Define a small threshold to check if x is close enough to goal
        float epsilon = 0.01f; // You can adjust this value as needed

        // Check if x is within the threshold of the goal
        if (Mathf.Abs(x - goal) < epsilon)
        {
            x = goal;  // Set x to goal for precise alignment
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            stepping_can = true;
        }
    }

    void MovmentPos()
    {
        if (m_up)
        {
            goal = transform.position.x + stepping_x;
            //Debug.Log($"DEBUG : + Goal is set to : {goal}");
            stepping_can = false;
            MaxCamMove.x = MaxCamMove.x - 1;
            MaxCamMove.y = MaxCamMove.y + 1;
        }
        else if (!m_up)
        {
            goal = transform.position.x - stepping_x;
            //Debug.Log($"DEBUG : - Goal is set to : {goal}");
            stepping_can = false;
            MaxCamMove.y = MaxCamMove.y - 1;
            MaxCamMove.x = MaxCamMove.x + 1;
        }
    }
}