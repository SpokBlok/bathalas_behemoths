using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelPicker : MonoBehaviour
{
    public GameObject steveModel;
    public GameObject mountedModel;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerStats.Instance.ruinsScene && PlayerStats.Instance.introDone)
        {
            steveModel.SetActive(true);
            mountedModel.SetActive(false);
        }
        else
        {
            steveModel.SetActive(false);
            mountedModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
