using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TammyHPBAr : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public Image healthBar;
    public Tambanokano tammy;
    public GameObject hpHUD;
    public float maxHP = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        tammy = GameObject.FindGameObjectWithTag("Tambanokano").GetComponent<Tambanokano>();
        HPText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.dead)
        {
            hpHUD.SetActive(false);
        }
        if(tammy == null)
        {
            if(GameObject.FindGameObjectWithTag("Tambanokano") != null)
            {
                tammy = GameObject.FindGameObjectWithTag("Tambanokano").GetComponent<Tambanokano>();
            }
        }
        if(HPText == null)
        {
            HPText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if(maxHP == 0f)
        {
            maxHP = tammy.health;
        }
        HPText.text = tammy.health.ToString("0") + "/" + maxHP + " HP";
        healthBar.fillAmount = tammy.health/maxHP;
    }
}
