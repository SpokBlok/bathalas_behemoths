using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Playables;

public class IntroScript : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject introScene;
    public GameObject introScene2;
    public GameObject instructions;
    
    private bool isInTrigger;
    [SerializeField] public PlayableDirector _cutsceneTimeline;

    // Start is called before the first frame update
    void Start()
    {
        isInTrigger = false;
        if(PlayerStats.Instance.introDone)
        {
            introScene.SetActive(false);
            introScene2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            if (PlayerStats.Instance.introDone == false)
            {
                _cutsceneTimeline.Play();
                dialogue.SetActive(true);
                instructions.SetActive(false);
                
                //SceneManager.LoadScene("OutdoorsSceneFinalCutscene");
                
            }
        }
    }

    // Called when exiting the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            QuestState.Instance.chickenSightTrigger = true;
            // popUp.gameObject.SetActive(false);
        }
    }
}
