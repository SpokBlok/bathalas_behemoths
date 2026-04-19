using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IdentifyChecker : MonoBehaviour
{
    public GameObject tammyNotif;
    public GameObject markyNotif;
    public GameObject apolakiNotif;
    [SerializeField] public PlayableDirector _cutsceneTimeline;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.tammyFound)
        {
            tammyNotif.SetActive(true);
        }
        else if(PlayerStats.Instance.markyFound)
        {
            markyNotif.SetActive(true);
        }
        else if(PlayerStats.Instance.apolakiFound)
        {
            apolakiNotif.SetActive(true);
            _cutsceneTimeline.Play();
            // Stop playing after one play of the cutscene
            _cutsceneTimeline.stopped += OnCutsceneStopped;
        }
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        _cutsceneTimeline.stopped -= OnCutsceneStopped;
        PlayerStats.Instance.apolakiFound = false;
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(false);
        apolakiNotif.SetActive(false);
        // Reload the scene after the cutscene finishes
        // SceneManager.LoadScene("RuinsScene Movement");
    }
}
