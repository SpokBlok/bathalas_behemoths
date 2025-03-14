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

            PlayerMovement playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            if(playerScript.isBerserk)
            {
                if(PlayerSkills.Instance.mainCharacterSkillCoroutine != null)
                {
                    PlayerSkills.Instance.StopCoroutine(PlayerSkills.Instance.mainCharacterSkillCoroutine);
                }
                PlayerStats.Instance.speedMultiplier = 1;
                playerScript.isBerserk = false;
            }
            bbSkill = FindObjectOfType<BathalasBlessing>();
            bbSkill.RechargeUsages(); // Calls recharge on the skill usage for BB - since it's a one-use skill
        }
    }
}
