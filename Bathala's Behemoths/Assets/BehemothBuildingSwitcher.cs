using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehemothBuildingSwitcher : MonoBehaviour
{
    public GameObject tammyBuilding;
    public GameObject markyBuilding;

    // Start is called before the first frame update
    void Start()
    {
        if(QuestState.Instance.tambanokanoDefeated)
        {
            tammyBuilding.SetActive(true);
        }
        else
        {
            tammyBuilding.SetActive(false);
        }

        if(QuestState.Instance.markupoDefeated)
        {
            markyBuilding.SetActive(true);
        }
        else
        {
            markyBuilding.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
