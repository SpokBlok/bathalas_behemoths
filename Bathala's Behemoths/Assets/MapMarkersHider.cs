using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarkersHider : MonoBehaviour
{
    public GameObject waypoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestState.Instance.pausedForDialogue)
        {
            waypoint.SetActive(false);
        }
        else
        {
            waypoint.SetActive(true);
        }
    }
}
