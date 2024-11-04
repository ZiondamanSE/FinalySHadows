using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSystem : MonoBehaviour
{
    BoxCollider2D bc2;

    public Transform p_pos;

    [Range(-1, 1)]
    public int cameraStepping;

    [HideInInspector] public bool m_Cam;
    private bool shitthebed;

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
            StartCoroutine(WaitAndExecute()); // Start the coroutine for waiting

            Debug.Log("DEBUG : Camera moving is Active");
            //shitthebed = true;
        }
    }

    private IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(1f); // Waits for 1 second
        shit(); // Calls the method after waiting
    }
    /**
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shitthebed)
        {
            shit();
            Debug.Log($"DEBUG : Camera Swich is Active, stepping set to : {cameraStepping}");
        }
        shitthebed = false;
    }
    */

    void shit()
    {

        if(p_pos.position.x > transform.position.x + 0.2)
        {
            cameraStepping = 1;
            m_Cam = false;
        }
        else if(p_pos.position.x > transform.position.x - 0.2f)
        {
            cameraStepping = -1;
            m_Cam = false;
        }
    }
}
