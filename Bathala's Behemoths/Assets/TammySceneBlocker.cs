using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TammySceneBlocker : MonoBehaviour
{
    public GameObject tammyDoor;
    public GameObject tammyModel;

    // Start is called before the first frame update
    void Start()
    {
        if(QuestState.Instance.tambanokanoFound)
        {
            tammyDoor.SetActive(true);
            tammyModel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            tammyDoor.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
