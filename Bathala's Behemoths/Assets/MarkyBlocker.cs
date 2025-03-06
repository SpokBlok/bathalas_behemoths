using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkyBlocker : MonoBehaviour
{
    public GameObject markyDoor;
    public GameObject markyModel;

    // Start is called before the first frame update
    void Start()
    {
        if(QuestState.Instance.markupoFound)
        {
            markyDoor.SetActive(true);
            markyModel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            markyDoor.SetActive(false);
        }
    }
}
