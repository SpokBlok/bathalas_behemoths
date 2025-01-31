using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI HPText;

    // public static HealthBar Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnEnteringUpgradeScreen += DisableUI;
        EventManager.OnExitingUpgradeScreen += EnableUI;
        HPText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        HPText.text = "HP: " + PlayerStats.Instance.currentHealth + "/" + PlayerStats.Instance.maxHealth;
    }

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject); // Keep this object across scenes
    //     }
    //     else
    //     {
    //         Destroy(gameObject); // Destroy duplicate instance
    //     }
    // }

    private void EnableUI()
    {
        gameObject.SetActive(true);
    }

    private void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
