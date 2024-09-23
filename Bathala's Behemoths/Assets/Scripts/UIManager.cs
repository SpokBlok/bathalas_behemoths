using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI basicAttackText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdatePlayerStatsUI();
    }

    private void UpdatePlayerStatsUI()
    {
        basicAttackText.text = "Basic Attack: " + PlayerStats.Instance.basicAttackDamage.ToString();
    }
}
