using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class markyHPBAr : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public Image healthBar;
    public MarkupoScript marky;
    public float maxHP = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        marky = GameObject.FindGameObjectWithTag("Markupo").GetComponent<MarkupoScript>();
        HPText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(marky == null)
        {
            if(GameObject.FindGameObjectWithTag("Markupo") != null)
            {
                marky = GameObject.FindGameObjectWithTag("Markupo").GetComponent<MarkupoScript>();
            }
        }
        if(HPText == null)
        {
            HPText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if(maxHP == 0f)
        {
            maxHP = marky.health;
        }
        HPText.text = marky.health + "/" + maxHP + " HP";
        healthBar.fillAmount = marky.health/maxHP;
    }
}
