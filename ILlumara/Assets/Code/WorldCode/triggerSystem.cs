using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSystem : MonoBehaviour
{
    [Range(-1, 1)]
    public int cameraStepping;

    [HideInInspector] public bool m_Cam;

    void Awake()
    {
        m_Cam = false;
        if (cameraStepping == 0)
            Debug.LogError("ERROR : Camera will not move");
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_Cam = true;
            Debug.Log("DEBUG : Camera moving is Active");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Cam = false;

        if (cameraStepping == 1)
            cameraStepping = -1;
        else if (cameraStepping == -1)
            cameraStepping = 1;

        Debug.Log($"DEBUG : Camera Swich is Active, stepping set to : {cameraStepping}");
    }
}
