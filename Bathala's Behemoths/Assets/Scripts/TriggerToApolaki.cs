using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToApolaki : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerStats.Instance.apolakiFound)
        {
            SceneManager.LoadScene("Apolaki");
            PlayerStats.Instance.apolakiScene = true;
            PlayerStats.Instance.ruinsScene = false;
            PlayerStats.Instance.ruinsVisitedOnce = true;
            PlayerStats.Instance.dead = false;
            PlayerStats.Instance.speed = 45;
        }
    }
}
