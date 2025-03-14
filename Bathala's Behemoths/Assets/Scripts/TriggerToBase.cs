using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToBase : MonoBehaviour
{
    public BathalasBlessing bbSkill;

    // Start is called before the first frame update
    void Start()
    {
        bbSkill = FindObjectOfType<BathalasBlessing>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("RuinsScene Movement");
            PlayerStats.Instance.introDone = true;
            PlayerStats.Instance.outdoorsScene = false;
            PlayerStats.Instance.ruinsScene = true;
            PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
            bbSkill.RechargeUsages(); // Calls recharge on the skill usage for BB - since it's a one-use skill
        }
    }
}
