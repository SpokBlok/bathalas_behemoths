using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyChecker : MonoBehaviour
{
    public GameObject tammyNotif;
    public GameObject markyNotif;
    public GameObject apolakiNotif;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.tammyFound)
        {
            tammyNotif.SetActive(true);
        }
        else if(PlayerStats.Instance.markyFound)
        {
            markyNotif.SetActive(true);
        }
        else if(PlayerStats.Instance.apolakiFound)
        {
            apolakiNotif.SetActive(true);
        }
    }
}
