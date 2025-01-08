using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCSkillsUIPanel : MonoBehaviour
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
}
