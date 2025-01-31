using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestBoardUIPanel : MonoBehaviour
{
    public GameObject tammyNotif;
    public GameObject markyNotif;

    private int selectedClue1Set;
    private int selectedClue2Set;
    private int selectedClue3Set;

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

    // Start is called before the first frame update
    void Start()
    {
        selectedClue1Set = 0;
        selectedClue2Set = 0;
        selectedClue3Set = 0;

        selectedClue1Image = transform.Find("LeftPanel/Selected Clue 1").GetComponentInChildren<Image>();
        selectedClue2Image = transform.Find("LeftPanel/Selected Clue 2").GetComponentInChildren<Image>();
        selectedClue3Image = transform.Find("LeftPanel/Selected Clue 3").GetComponentInChildren<Image>();
    
    }

    void OnEnable()
    {
        if(PlayerStats.Instance.clue1 == true)
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

        if(PlayerStats.Instance.clue2 == true)
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

        if(PlayerStats.Instance.clue3 == true)
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

        if(PlayerStats.Instance.clue4 == true)
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

        if(PlayerStats.Instance.clue5 == true)
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

        if(PlayerStats.Instance.clue6 == true)
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

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);

        EventManager.Instance.InvokeOnEnteringUpgradeScreen();
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
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

    public void AssignMarkupoClue1()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue1Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue1Set = 1;
        if (selectedClue1Image.sprite == selectedClue2Image.sprite)
        {
            selectedClue2Set = 0;
            selectedClue2Image.sprite = null;
        }

        if (selectedClue1Image.sprite == selectedClue3Image.sprite)
        {
            selectedClue3Set = 0;
            selectedClue3Image.sprite = null;
        }
    }

    public void AssignMarkupoClue2()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue2Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue2Set = 1;
        if (selectedClue2Image.sprite == selectedClue1Image.sprite)
        {
            selectedClue1Set = 0;
            selectedClue1Image.sprite = null;
        }

        if (selectedClue2Image.sprite == selectedClue3Image.sprite)
        {
            selectedClue3Set = 0;
            selectedClue3Image.sprite = null;
        }
    }

    public void AssignMarkupoClue3()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue3Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue3Set = 1;
        if (selectedClue3Image.sprite == selectedClue1Image.sprite)
        {
            selectedClue1Set = 0;
            selectedClue1Image.sprite = null;
        }

        if (selectedClue3Image.sprite == selectedClue2Image.sprite)
        {
            selectedClue2Set = 0;
            selectedClue2Image.sprite = null;
        }
    }

    public void AssignTambanokanoClue1()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue1Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue1Set = 2;
        if (selectedClue1Image.sprite == selectedClue2Image.sprite)
        {
            selectedClue2Set = 0;
            selectedClue2Image.sprite = null;
        }

        if (selectedClue1Image.sprite == selectedClue3Image.sprite)
        {
            selectedClue3Set = 0;
            selectedClue3Image.sprite = null;
        }
    }

    public void AssignTambanokanoClue2()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue2Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue2Set = 2;
        if (selectedClue2Image.sprite == selectedClue1Image.sprite)
        {
            selectedClue1Set = 0;
            selectedClue1Image.sprite = null;
        }

        if (selectedClue2Image.sprite == selectedClue3Image.sprite)
        {
            selectedClue3Set = 0;
            selectedClue3Image.sprite = null;
        }
    }

    public void AssignTambanokanoClue3()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        selectedClue3Image.sprite = clickedObject.transform.parent.GetComponentInChildren<Image>().sprite;
        selectedClue3Set = 2;
        if (selectedClue3Image.sprite == selectedClue1Image.sprite)
        {
            selectedClue1Set = 0;
            selectedClue1Image.sprite = null;
        }

        if (selectedClue3Image.sprite == selectedClue2Image.sprite)
        {
            selectedClue2Set = 0;
            selectedClue2Image.sprite = null;
        }
    }

    public void ClueCheck()
    {
        if (selectedClue1Set == selectedClue2Set && selectedClue2Set == selectedClue3Set)
        {
            if (selectedClue1Set == 0)
            {
                Debug.Log("No clues selected");
            }
            if (selectedClue1Set == 1)
            {
                //markupo quest unlock
                QuestState.Instance.markupoFound = true;
                DisablePanel();
                markyNotif.SetActive(true);
                Debug.Log("Markupo found");
            }
            else if (selectedClue1Set == 2)
            {
                //tambanokano quest unlock
                QuestState.Instance.tambanokanoFound = true;
                DisablePanel();
                tammyNotif.SetActive(true);
                Debug.Log("Tammy found");
            }
        }
        else
        {
            Debug.Log("Wrong clues");
        }
    }
}
