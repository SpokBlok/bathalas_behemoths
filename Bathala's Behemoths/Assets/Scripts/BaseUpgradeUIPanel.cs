using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeUIPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DisplaySkill1()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Skill 1")
            {
                child.gameObject.SetActive(true);
            }
            else { 
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Skill1Upgrade()
    {
        PlayerStats.Instance.AddSpeed(10);
    }

    public void DisplaySkill2()
    {
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == "Skill 2")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void Skill2Upgrade()
    {
        PlayerStats.Instance.AddSpeed(-10);
    }
}
