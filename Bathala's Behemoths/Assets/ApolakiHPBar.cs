using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApolakiHPBar : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public Image healthBar;
    public Apolaki apolaki;
    public GameObject hpHUD;
    public float maxHP = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        apolaki = GameObject.FindGameObjectWithTag("Apolaki").GetComponent<Apolaki>();
        HPText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.dead)
        {
            hpHUD.SetActive(false);
        }
        if(apolaki == null)
        {
            if(GameObject.FindGameObjectWithTag("Apolaki") != null)
            {
                apolaki = GameObject.FindGameObjectWithTag("Apolaki").GetComponent<Apolaki>();
            }
        }
        if(HPText == null)
        {
            HPText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if(maxHP == 0f)
        {
            maxHP = apolaki.health;
        }
        HPText.text = apolaki.health.ToString("0") + "/" + maxHP + " HP";
        healthBar.fillAmount = apolaki.health/maxHP;
    }
}
