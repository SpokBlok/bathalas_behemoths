using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestBoardUIPanel : MonoBehaviour
{
    private int selectedClue1Set;
    private int selectedClue2Set;
    private int selectedClue3Set;

    private Image selectedClue1Image;
    private Image selectedClue2Image;
    private Image selectedClue3Image;

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
                Debug.Log("Markupo found");
            }
            else if (selectedClue1Set == 2)
            {
                //tambanokano quest unlock
                Debug.Log("Tammy found");
            }
        }
        else
        {
            Debug.Log("Wrong clues");
        }
    }
}
