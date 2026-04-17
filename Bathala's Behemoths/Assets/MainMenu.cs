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
        // Destroy (GameObject.Find("PlayerStats"));
        // Destroy (GameObject.Find("QuestState"));
        // Destroy (GameObject.Find("PlayerSkills"));
    }
    
    void OnAwake()
    {
        Destroy (GameObject.Find("UICanvas"));
        // Destroy (GameObject.Find("PauseSystem"));
        // Destroy (GameObject.Find("PlayerStats"));
        // Destroy (GameObject.Find("QuestState"));
        // Destroy (GameObject.Find("PlayerSkills"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressStart()
    {
        PlayerStats.Instance.ClearSave();
        QuestState.Instance.ClearSave();
        PlayerStats.Instance.SetScenePosition();
        SceneManager.LoadScene(1);
    }

    public void pressLoad()
    {   
        if(!SaveSystem.Instance.hasSave) {return;}

        if(SaveSystem.Instance != null)
        {
            SaveSystem.Instance.Load();
        }

        if(PlayerStats.Instance.outdoorsScene)
        {
            SceneManager.LoadScene("OutdoorsSceneFinal");
        }
        else if(PlayerStats.Instance.ruinsScene)
        {
            SceneManager.LoadScene("RuinsScene Movement");
        }
        else if(PlayerStats.Instance.tammyScene)
        {
            SceneManager.LoadScene("Tammy");
        }
        else if(PlayerStats.Instance.markyScene)
        {
            SceneManager.LoadScene("Marky");
        }
        else if(PlayerStats.Instance.apolakiScene)
        {
            SceneManager.LoadScene("Apolaki");
        }
        else
        {
            SceneManager.LoadScene("OutdoorsSceneFinal");
        }
    }

    public void pressCredits()
    {
        SceneManager.LoadScene(7);
    }

    public void pressQuit()
    {
        Application.Quit();
    }
}
