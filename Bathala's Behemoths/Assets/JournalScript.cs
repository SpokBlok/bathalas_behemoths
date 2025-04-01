using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        // QuestState.Instance.pausedForDialogue = true;
        hud = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = hud.gameObject.transform.position;
        obtainedOGPos = true;
        hud.gameObject.transform.position = new Vector3(10000, 10000, 10000);

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
        if (context.performed && !isPanelUp)
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
}
