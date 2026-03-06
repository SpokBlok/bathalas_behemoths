using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BehemothSwitchUIPanel : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerSkills playerSkills;
    private QuestState questState;
    
    public GameObject hud;
    public Vector3 originalHUDPos;
    public bool obtainedOGPos;

    public Button tammyButton;
    public Button markyButton;
    public TextMeshProUGUI tammyText;
    public TextMeshProUGUI markyText;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.Instance;
        playerSkills = PlayerSkills.Instance;
        questState = QuestState.Instance;
        
        hud = GameObject.FindGameObjectWithTag("HUD");

        if(questState.tambanokanoDefeated)
        {
            tammyButton.interactable = true;
            tammyButton.GetComponent<Image>().color = new Color(197, 197, 197, 255); 
            tammyText.text = "Tambanokano";
        }
        else
        {
            tammyButton.interactable = false;
            tammyButton.GetComponent<Image>().color = Color.black; 
            tammyText.text = "Behemoth Yet To Be Defeated...";
        }

        if(questState.markupoDefeated)
        {
            markyButton.interactable = true;
            markyButton.GetComponent<Image>().color = new Color(197, 197, 197, 255); 
            markyText.text = "Markupo";
        }
        else
        {
            markyButton.interactable = false;
            markyButton.GetComponent<Image>().color = Color.black; 
            markyText.text = "Behemoth Yet To Be Defeated...";
        }
    }

    // is called when object is enabled
    void OnEnable()
    {
        if(questState.tambanokanoDefeated)
        {
            tammyButton.interactable = true;
            tammyButton.GetComponent<Image>().color = new Color(197, 197, 197, 255); 
            tammyText.text = "Tambanokano";
        }
        else
        {
            tammyButton.interactable = false;
            tammyButton.GetComponent<Image>().color = Color.black; 
            tammyText.text = "Behemoth Yet To Be Defeated...";
        }

        if(questState.markupoDefeated)
        {
            markyButton.interactable = true;
            markyButton.GetComponent<Image>().color = new Color(197, 197, 197, 255); 
            markyText.text = "Markupo";
        }
        else
        {
            markyButton.interactable = false;
            markyButton.GetComponent<Image>().color = Color.black; 
            markyText.text = "Behemoth Yet To Be Defeated...";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Equip Manny as the behemoth model
    public void equipManny()
    {
        playerStats.playerModelIndex = 1;
    }

    // Equip Tammy as the behemoth model
    public void equipTammy()
    {
        playerStats.playerModelIndex = 2;
    }

    // Equip Marky as the behemoth model
    public void equipMarky()
    {
        playerStats.playerModelIndex = 3;
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        QuestState.Instance.pausedForDialogue = true;
        hud = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = hud.gameObject.transform.position;
        obtainedOGPos = true;
        hud.gameObject.transform.position = new Vector3(10000, 10000, 10000);
        EventManager.Instance.InvokeOnEnteringUpgradeScreen();

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
        QuestState.Instance.pausedForDialogue = false;
        hud = GameObject.FindGameObjectWithTag("HUD");

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

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
}
