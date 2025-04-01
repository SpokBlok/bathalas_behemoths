using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class JournalScript : MonoBehaviour
{
    public static JournalScript Instance { get; private set; }

    private Image selectedClue1Image;
    private Image selectedClue2Image;
    private Image selectedClue3Image;

    [SerializeField] GameObject clue1;
    [SerializeField] GameObject clue2;
    [SerializeField] GameObject clue3;
    [SerializeField] GameObject clue4;
    [SerializeField] GameObject clue5;
    [SerializeField] GameObject clue6;
    [SerializeField] GameObject clue1Tracking;
    [SerializeField] GameObject clue2Tracking;
    [SerializeField] GameObject clue3Tracking;
    [SerializeField] GameObject clue4Tracking;
    [SerializeField] GameObject clue5Tracking;
    [SerializeField] GameObject clue6Tracking;

    public GameObject hud;
    public Vector3 originalHUDPos;
    public bool obtainedOGPos;

    private bool isPanelUp;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD");
        isPanelUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    void OnEnable()
    {
        if (PlayerStats.Instance.clue1 == true)
        {
            clue1.SetActive(true);
            clue1Tracking.SetActive(false);
            Debug.Log("Clue 1 is true");
        }
        else
        {
            clue1.SetActive(false);
            clue1Tracking.SetActive(true);
        }

        if (PlayerStats.Instance.clue2 == true)
        {
            clue2.SetActive(true);
            clue2Tracking.SetActive(false);
            Debug.Log("Clue 2 is true");
        }
        else
        {
            clue2.SetActive(false);
            clue2Tracking.SetActive(true);
        }

        if (PlayerStats.Instance.clue3 == true)
        {
            clue3.SetActive(true);
            clue3Tracking.SetActive(false);
            Debug.Log("Clue 3 is true");
        }
        else
        {
            clue3.SetActive(false);
            clue3Tracking.SetActive(true);
        }

        if (PlayerStats.Instance.clue4 == true)
        {
            clue4.SetActive(true);
            clue4Tracking.SetActive(false);
            Debug.Log("Clue 4 is true");
        }
        else
        {
            clue4.SetActive(false);
            clue4Tracking.SetActive(true);
        }

        if (PlayerStats.Instance.clue5 == true)
        {
            clue5.SetActive(true);
            clue5Tracking.SetActive(false);
            Debug.Log("Clue 5 is true");
        }
        else
        {
            clue5.SetActive(false);
            clue5Tracking.SetActive(true);
        }

        if (PlayerStats.Instance.clue6 == true)
        {
            clue6.SetActive(true);
            clue6Tracking.SetActive(false);
            Debug.Log("Clue 6 is true");
        }
        else
        {
            clue6.SetActive(false);
            clue6Tracking.SetActive(true);
        }

    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        // QuestState.Instance.pausedForDialogue = true;
        hud = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = hud.gameObject.transform.position;
        obtainedOGPos = true;
        hud.gameObject.transform.position = new Vector3(10000, 10000, 10000);

        QuestState.Instance.pausedForDialogue = true;
        EventManager.Instance.InvokeOnEnteringUpgradeScreen();
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        QuestState.Instance.pausedForDialogue = false;
        hud = GameObject.FindGameObjectWithTag("HUD");
        // hud.gameObject.transform.position = new Vector3(859.20f, 640.80f, 0.00f);
        if (obtainedOGPos)
        {
            hud.gameObject.transform.position = originalHUDPos;
        }
        QuestState.Instance.pausedForDialogue = false;
        EventManager.Instance.InvokeOnExitingUpgradeScreen();
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DisplayClue()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        Transform panel = transform.Find("RightPanel");
        foreach (Transform child in panel)
        {
            if (child.name == clickedObject.transform.parent.name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        // Check for the key press only when inside the trigger
        if (context.performed && !isPanelUp && !QuestState.Instance.pausedForDialogue)
        {
            isPanelUp = true;
            EnablePanel();
        }
        else if (context.performed && isPanelUp)
        {
            isPanelUp = false;
            DisablePanel();
        }
    }

    public void SetActiveQuest()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        string questNumber = clickedObject.GetComponentInChildren<TextMeshProUGUI>().name;
        PlayerStats.Instance.ActiveQuestReset();
        switch (questNumber)
        {
            case "1":
                PlayerStats.Instance.activeQuest1 = true;
                break;
            case "2":
                PlayerStats.Instance.activeQuest2 = true;
                break;
            case "3":
                PlayerStats.Instance.activeQuest3 = true;
                break;
            case "4":
                PlayerStats.Instance.activeQuest4 = true;
                break;
            case "5":
                PlayerStats.Instance.activeQuest5 = true;
                break;
            case "6":
                PlayerStats.Instance.activeQuest6 = true;
                break;
        }
    }
}
