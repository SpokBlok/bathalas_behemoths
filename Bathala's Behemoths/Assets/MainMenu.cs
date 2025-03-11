using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy (GameObject.Find("UI Canvas"));
        Destroy (GameObject.Find("PlayerStats"));
        Destroy (GameObject.Find("QuestState"));
        Destroy (GameObject.Find("PlayerSkills"));
    }
    
    void OnAwake()
    {
        Destroy (GameObject.Find("UICanvas"));
        Destroy (GameObject.Find("PauseSystem"));
        Destroy (GameObject.Find("PlayerStats"));
        Destroy (GameObject.Find("QuestState"));
        Destroy (GameObject.Find("PlayerSkills"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressStart()
    {
        SceneManager.LoadScene(1);
    }

    public void pressQuit()
    {
        Application.Quit();
    }
}
