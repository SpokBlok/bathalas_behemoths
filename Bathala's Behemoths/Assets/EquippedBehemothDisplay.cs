using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedBehemothDisplay : MonoBehaviour
{
    public GameObject mannyDisplay;
    public GameObject tammyDisplay;
    public GameObject markyDisplay;

    private int currentEquippedBehemoth = 1; // 1 for Manny, 2 for Tammy, 3 for Marky

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.playerModelIndex == 1 && currentEquippedBehemoth != 1)
        {
            switchToManny();
        }
        else if(PlayerStats.Instance.playerModelIndex == 2 && currentEquippedBehemoth != 2)
        {
            switchToTammy();
        }
        else if(PlayerStats.Instance.playerModelIndex == 3 && currentEquippedBehemoth != 3)
        {
            switchToMarky();
        }
    }

    public void switchToManny()
    {
        mannyDisplay.SetActive(true);
        tammyDisplay.SetActive(false);
        markyDisplay.SetActive(false);
        currentEquippedBehemoth = 1;
    }

    public void switchToTammy()
    {
        mannyDisplay.SetActive(false);
        tammyDisplay.SetActive(true);
        markyDisplay.SetActive(false);
        currentEquippedBehemoth = 2;
    }

    public void switchToMarky()
    {
        mannyDisplay.SetActive(false);
        tammyDisplay.SetActive(false);
        markyDisplay.SetActive(true);
        currentEquippedBehemoth = 3;
    }
}
