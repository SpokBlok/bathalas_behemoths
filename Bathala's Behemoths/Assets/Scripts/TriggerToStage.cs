using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToStage : MonoBehaviour
{
    public GameObject sceneDoor;
    public GameObject playerOutdoors;

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
            SceneManager.LoadScene("OutdoorsSceneFinal");
            PlayerStats.Instance.outdoorsScene = true;
            PlayerStats.Instance.ruinsScene = false;
            PlayerStats.Instance.speed = 45;
        }
    }
}
