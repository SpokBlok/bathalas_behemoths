using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToApolakiVisibility : MonoBehaviour
{
    [SerializeField] GameObject translabel;
    [SerializeField] GameObject transition;

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.apolakiFound)
        {
            translabel.SetActive(true);
            transition.SetActive(true);
        }
        else
        {
            translabel.SetActive(false);
            transition.SetActive(false);
        }
    }
}
