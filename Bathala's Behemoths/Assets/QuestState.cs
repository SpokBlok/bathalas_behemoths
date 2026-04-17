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
    public bool tambanokanoDefeated;
    public bool markupoDefeated;
    public bool apolakiDefeated;

    public bool goodEnding;

    public bool pausedForDialogue = false;
    public bool menuActive = false;
    public bool pauseActive = false;

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
        desponDwendeSightTrigger = false;
        chickenSightTrigger = false;
        fluteGet = false;
        lssDwendeRepeat = false;
        fluteQuestTrigger = false;
        fluteSightTrigger = false;
        moonSightingTrigger = false;
        moonQuestTrigger = false;

        tambanokanoFound = false;
        markupoFound = false;

        // Other booleans default to false by design
        frightenedDwendeRepeat = false;
        charredTreeRepeat = false;
        washedUpFenceRepeat = false;
        desponDwendeRepeat = false;
        footprintsRepeat = false;
        dwendeMomRepeat = false;
        moonBraceletRepeat = false;
        chickenRepeat = false;
        fluteChestRepeat = false;
        vetFarmerRepeat = false;
        gardenerRepeat = false;
        fertilizerRepeat = false;

        lssDwendeRan = false;
        questTutorialTrigger = false;
        skillsTutorialTrigger = false;

        tambanokanoDefeated = false;
        markupoDefeated = false;
        apolakiDefeated = false;

        goodEnding = false;

        pausedForDialogue = false;
        menuActive = false;
        pauseActive = false;
    }

    public void ClearSave()
    {
        // Reset all state to initial defaults
        InitializeQuestState();
    }

    public void Save(ref QuestStateSaveData data)
    {
        data.frightenedDwendeRepeat = frightenedDwendeRepeat;
        data.charredTreeRepeat = charredTreeRepeat;
        data.washedUpFenceRepeat = washedUpFenceRepeat;
        data.desponDwendeRepeat = desponDwendeRepeat;
        data.footprintsRepeat = footprintsRepeat;
        data.dwendeMomRepeat = dwendeMomRepeat;
        data.moonNPCRepeat = moonNPCRepeat;
        data.moonBraceletRepeat = moonBraceletRepeat;
        data.chickenRepeat = chickenRepeat;
        data.lssDwendeRepeat = lssDwendeRepeat;
        data.fluteChestRepeat = fluteChestRepeat;
        data.vetFarmerRepeat = vetFarmerRepeat;
        data.gardenerRepeat = gardenerRepeat;
        data.fertilizerRepeat = fertilizerRepeat;

        data.desponDwendeSightTrigger = desponDwendeSightTrigger;
        data.chickenSightTrigger = chickenSightTrigger;
        data.moonChunkGet = moonChunkGet;
        data.moonQuestEnded = moonQuestEnded;
        data.moonQuestTrigger = moonQuestTrigger;
        data.moonSightingTrigger = moonSightingTrigger;
        data.fluteQuestTrigger = fluteQuestTrigger;
        data.fluteGet = fluteGet;
        data.lssDwendeRan = lssDwendeRan;
        data.fluteSightTrigger = fluteSightTrigger;
        data.questTutorialTrigger = questTutorialTrigger;
        data.skillsTutorialTrigger = skillsTutorialTrigger;

        data.tambanokanoFound = tambanokanoFound;
        data.markupoFound = markupoFound;
        data.tambanokanoDefeated = tambanokanoDefeated;
        data.markupoDefeated = markupoDefeated;
        data.apolakiDefeated = apolakiDefeated;

        data.goodEnding = goodEnding;
        data.pausedForDialogue = false;
        data.menuActive = false;
        data.pauseActive = false;
    }

    public void Load(QuestStateSaveData data)
    {
        frightenedDwendeRepeat = data.frightenedDwendeRepeat;
        charredTreeRepeat = data.charredTreeRepeat;
        washedUpFenceRepeat = data.washedUpFenceRepeat;
        desponDwendeRepeat = data.desponDwendeRepeat;
        footprintsRepeat = data.footprintsRepeat;
        dwendeMomRepeat = data.dwendeMomRepeat;
        moonNPCRepeat = data.moonNPCRepeat;
        moonBraceletRepeat = data.moonBraceletRepeat;
        chickenRepeat = data.chickenRepeat;
        lssDwendeRepeat = data.lssDwendeRepeat;
        fluteChestRepeat = data.fluteChestRepeat;
        vetFarmerRepeat = data.vetFarmerRepeat;
        gardenerRepeat = data.gardenerRepeat;
        fertilizerRepeat = data.fertilizerRepeat;

        desponDwendeSightTrigger = data.desponDwendeSightTrigger;
        chickenSightTrigger = data.chickenSightTrigger;
        moonChunkGet = data.moonChunkGet;
        moonQuestEnded = data.moonQuestEnded;
        moonQuestTrigger = data.moonQuestTrigger;
        moonSightingTrigger = data.moonSightingTrigger;
        fluteQuestTrigger = data.fluteQuestTrigger;
        fluteGet = data.fluteGet;
        lssDwendeRan = data.lssDwendeRan;
        fluteSightTrigger = data.fluteSightTrigger;
        questTutorialTrigger = data.questTutorialTrigger;
        skillsTutorialTrigger = data.skillsTutorialTrigger;

        tambanokanoFound = data.tambanokanoFound;
        markupoFound = data.markupoFound;
        tambanokanoDefeated = data.tambanokanoDefeated;
        markupoDefeated = data.markupoDefeated;
        apolakiDefeated = data.apolakiDefeated;

        goodEnding = data.goodEnding;
        pausedForDialogue = false;
        menuActive = false;
        pauseActive = false;
    }
}

[System.Serializable]
public struct QuestStateSaveData
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

    public bool tambanokanoFound;
    public bool markupoFound;
    public bool tambanokanoDefeated;
    public bool markupoDefeated;
    public bool apolakiDefeated;

    public bool goodEnding;
    public bool pausedForDialogue;
    public bool menuActive;
    public bool pauseActive;
}
