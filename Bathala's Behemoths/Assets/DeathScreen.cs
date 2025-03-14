using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressContinue()
    {
        PlayerStats.Instance.tammyScene = false;
        PlayerStats.Instance.markyScene = false;
        PlayerStats.Instance.outdoorsScene = false;
        PlayerStats.Instance.ruinsScene = true;
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
        PlayerStats.Instance.speedMultiplier = 1;
        BathalasBlessing bbSkill = FindObjectOfType<BathalasBlessing>();
        bbSkill.RechargeUsages();
        SceneManager.LoadScene(2);
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
