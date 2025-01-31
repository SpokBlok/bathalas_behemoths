using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkyBlocker : MonoBehaviour
{
    public GameObject markyDoor;

    // Start is called before the first frame update
    void Start()
    {
        if(QuestState.Instance.markupoFound)
        {
            markyDoor.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            markyDoor.SetActive(false);
        }
    }
}
