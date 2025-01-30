using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenClueRadius : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<ChickenClue>().ChangeState(ChickenState.Moving);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<ChickenClue>().ChangeState(ChickenState.Idle);
        }
    }

    public void TriggerCheck()
    {
        if (GetComponent<Collider>().bounds.Contains(player.transform.position))
        {
            GetComponentInParent<ChickenClue>().ChangeState(ChickenState.Moving);
        }
        else
        {
            GetComponentInParent<ChickenClue>().ChangeState(ChickenState.Idle);
        }
    }
}
