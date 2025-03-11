using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestState : MonoBehaviour
{
    public bool frightenedDwendeRepeat;
    public bool charredTreeRepeat;
    public bool washedUpFenceRepeat;
    public bool desponDwendeRepeat;
    public bool footprintsRepeat;
    public bool dwendeMomRepeat;
    public bool moonNPCRepeat;
    public bool moonBraceletRepeat;
    public bool chickenRepeat;
    public bool lssDwendeRepeat;
    public bool fluteChestRepeat;
    public bool vetFarmerRepeat;
    public bool gardenerRepeat;
    public bool fertilizerRepeat;

    public bool desponDwendeSightTrigger;
    public bool chickenSightTrigger;
    public bool moonChunkGet;
    public bool moonQuestEnded;
    public bool moonQuestTrigger;
    public bool moonSightingTrigger;
    public bool fluteQuestTrigger;
    public bool fluteGet;
    public bool lssDwendeRan;
    public bool fluteSightTrigger;
    public bool questTutorialTrigger;
    public bool skillsTutorialTrigger;

    // Boss Scene Triggers
    public bool tambanokanoFound;
    public bool markupoFound;

    public bool pausedForDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static QuestState Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            InitializeQuestState();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void InitializeQuestState()
    {
        // Set default values
        moonQuestEnded = false;
        moonChunkGet = false;
        moonNPCRepeat = false;
        chickenSightTrigger = false;
        fluteGet = false;
        lssDwendeRepeat = false;
        fluteQuestTrigger = false;
        fluteSightTrigger = false;

        tambanokanoFound = false;
        markupoFound = false;
    }
}
