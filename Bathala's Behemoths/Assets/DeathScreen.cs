using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        HUDHider.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if(!UnityEngine.Cursor.visible)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
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
        QuestState.Instance.menuActive = false;
        HUDHider.Reset();
        BathalasBlessing bbSkill = FindObjectOfType<BathalasBlessing>();
        bbSkill.RechargeUsages();
        PlayerStats.Instance.SetScenePosition();
        SceneManager.LoadScene(3);
    }

    public void PressQuitToDesktop()
    {
        QuestState.Instance.menuActive = false;
        HUDHider.Reset();
        Application.Quit();
    }

    public void PressQuitToMenu()
    {
        QuestState.Instance.menuActive = false;
        HUDHider.Reset();
        SceneManager.LoadScene(0);
    }
}
