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
    }

    public void Skill1Upgrade()
    {
        PlayerStats.Instance.AddSpeed(10);
    }
}
