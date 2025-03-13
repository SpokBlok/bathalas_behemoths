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
        if(maxHP == 0f)
        {
            maxHP = tammy.health;
        }
        HPText.text = "HP: " + tammy.health + "/" + maxHP + " HP";
        healthBar.fillAmount = tammy.health/maxHP;
    }
}
