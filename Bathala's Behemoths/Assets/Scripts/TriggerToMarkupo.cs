using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToMarkupo : MonoBehaviour
{
    public GameObject markyHPBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(markyHPBar != null)
            {
                markyHPBar.SetActive(true);
            }
            PlayerStats.Instance.markyScene = true;
            PlayerStats.Instance.outdoorsScene = false;
            PlayerStats.Instance.speedMultiplier = 1.5f;
            SceneManager.LoadScene("Marky");
        }
    }
}
