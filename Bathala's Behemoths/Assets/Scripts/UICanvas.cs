using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static UICanvas Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    // private void Awake()
    // {
    //     if (FindObjectsOfType<UICanvas>().Length > 1)
    //     {
    //         // Destroy(gameObject);
    //         // Destroy(FindObjectsOfType<UICanvas>()[0])
    //         return;
    //     }

    //     DontDestroyOnLoad(gameObject);
    // }
}
