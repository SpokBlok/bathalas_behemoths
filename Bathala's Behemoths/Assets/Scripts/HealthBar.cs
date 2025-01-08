using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI HPText;

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

    private void EnableUI()
    {
        gameObject.SetActive(true);
    }

    private void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
