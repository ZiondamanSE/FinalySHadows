using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTrans : MonoBehaviour
{
    [SerializeField] public Image ip;

    private void Awake()
    {
        StartCoroutine(start_Fade());
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(End_Fade());
        }
    }

    IEnumerator End_Fade()
    {
        float Elapsedtime = 0;
        float Duriashon = 1;

        while (Elapsedtime < Duriashon)
        {
            Color imge_Color = ip.color;
            imge_Color.a = Mathf.Lerp(0f, 1f, Elapsedtime / Duriashon);
            ip.color = imge_Color;
            Elapsedtime += Time.fixedDeltaTime;
            yield return null;
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    IEnumerator start_Fade()
    {
        float Elapsedtime = 0;
        float Duriashon = 4;

        while (Elapsedtime < Duriashon)
        {
            Color imge_Color = ip.color;
            imge_Color.a = Mathf.Lerp(1f, 0f, Elapsedtime / Duriashon);
            ip.color = imge_Color;
            Elapsedtime += Time.fixedDeltaTime;
            yield return null;
            Time.timeScale = Mathf.Lerp(1f, 0f, Elapsedtime / Duriashon);
        }
        Time.timeScale = 1f; 
        
    }
}
