using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy (GameObject.Find("UI Canvas"));
        Destroy (GameObject.Find("PlayerStats"));
        Destroy (GameObject.Find("QuestState"));
        Destroy (GameObject.Find("PlayerSkills"));
        
        StartCoroutine(StartPrologue());
    }

    void OnAwake()
    {
        Destroy (GameObject.Find("UICanvas"));
        Destroy (GameObject.Find("PauseSystem"));
        Destroy (GameObject.Find("PlayerStats"));
        Destroy (GameObject.Find("QuestState"));
        Destroy (GameObject.Find("PlayerSkills"));

        StartCoroutine(StartPrologue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartPrologue()
    {
        yield return new WaitForSeconds(53f);
        SceneManager.LoadScene(0);
    }

    public void pressBBMM()
    {
        SceneManager.LoadScene(0);
    }
}
