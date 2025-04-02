using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    private GameObject HUD;
    private Vector3 originalHUDPos;
    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = HUD.gameObject.transform.position;
        HUD.gameObject.transform.position = new Vector3(10000, 10000, 10000);
    }

    // Update is called once per frame
    void Update()
    {
        if(HUD == null)
        {
            HUD = GameObject.FindGameObjectWithTag("HUD");
        }
    }

    public void PressContinue()
    {
        PlayerStats.Instance.tammyScene = false;
        PlayerStats.Instance.markyScene = false;
        PlayerStats.Instance.outdoorsScene = false;
        PlayerStats.Instance.ruinsScene = true;
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
        PlayerStats.Instance.speedMultiplier = 1;
        QuestState.Instance.pausedForDialogue = false;
        HUD = GameObject.FindGameObjectWithTag("HUD");
        if(HUD != null)
        {
            HUD.gameObject.transform.position = originalHUDPos;
        }
        BathalasBlessing bbSkill = FindObjectOfType<BathalasBlessing>();
        bbSkill.RechargeUsages();
        SceneManager.LoadScene(3);
    }

    public void PressQuitToDesktop()
    {
        Application.Quit();
    }

    public void PressQuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
