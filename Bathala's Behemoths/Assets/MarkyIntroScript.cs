using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MarkyIntroScript : MonoBehaviour
{
    [SerializeField] public PlayableDirector cutsceneTimeline;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneTimeline.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        cutsceneTimeline.stopped -= OnPlayableDirectorStopped;
        SceneManager.LoadScene("Marky");
    }

}
