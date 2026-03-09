using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehemothBuildingChillers : MonoBehaviour
{
    public GameObject mannyDisplay;
    public GameObject tammyDisplay;
    public GameObject markyDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchToManny()
    {
        mannyDisplay.SetActive(true);
        tammyDisplay.SetActive(false);
        markyDisplay.SetActive(false);
    }

    public void switchToTammy()
    {
        mannyDisplay.SetActive(false);
        tammyDisplay.SetActive(true);
        markyDisplay.SetActive(false);
    }

    public void switchToMarky()
    {
        mannyDisplay.SetActive(false);
        tammyDisplay.SetActive(false);
        markyDisplay.SetActive(true);
    }
}
