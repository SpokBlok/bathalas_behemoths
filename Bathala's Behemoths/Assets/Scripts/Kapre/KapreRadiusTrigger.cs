using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapreRadiusTrigger : MonoBehaviour
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
        if (GetComponentInParent<KapreMob>().kapreState != KapreState.Stunned)
        {
            GetComponentInParent<KapreMob>().ChangeState(KapreState.Moving);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetComponentInParent<KapreMob>().kapreState != KapreState.Stunned)
            {
                GetComponentInParent<KapreMob>().ChangeState(KapreState.Idle);
            }
        }
    }

    public void TriggerCheck()
    {
        if (GetComponent<Collider>().bounds.Contains(player.transform.position))
        {
            GetComponentInParent<KapreMob>().ChangeState(KapreState.Moving);
        }
        else
        {
            GetComponentInParent<KapreMob>().ChangeState(KapreState.Idle);
        }
    }
}
