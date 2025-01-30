using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToBase : MonoBehaviour
{
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
            SceneManager.LoadScene("RuinsScene Movement");
            PlayerStats.Instance.introDone = true;
            PlayerStats.Instance.outdoorsScene = false;
            PlayerStats.Instance.ruinsScene = true;

        }
    }
}
