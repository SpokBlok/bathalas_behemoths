using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TammyTamedScript : MonoBehaviour
{
    [SerializeField] public PlayableDirector cutsceneTimeline;
    public BathalasBlessing bbSkill;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneTimeline.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        cutsceneTimeline.stopped -= OnPlayableDirectorStopped;
        GoToBase();
    }

    void GoToBase()
    {
        SceneManager.LoadScene("RuinsScene Movement");
        PlayerStats.Instance.introDone = true;
        PlayerStats.Instance.outdoorsScene = false;
        PlayerStats.Instance.ruinsScene = true;
        PlayerStats.Instance.tammyScene = false;
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;

        PlayerMovement playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if(playerScript.isBerserk)
        {
            PlayerStats.Instance.speedMultiplier = 1;
            playerScript.isBerserk = false;
        }
        bbSkill = FindObjectOfType<BathalasBlessing>();
        bbSkill.RechargeUsages(); // Calls recharge on the skill usage for BB - since it's a one-use skill
        
        if(PlayerSkills.Instance.mainCharacterSkillCoroutine != null)
        {
            StopCoroutine(PlayerSkills.Instance.mainCharacterSkillCoroutine);
            PlayerSkills.Instance.mainCharacterSkillCoroutine = null;
        }
        
        if(PlayerSkills.Instance.behemothSkillQCoroutine != null)
        {
            StopCoroutine(PlayerSkills.Instance.behemothSkillQCoroutine);
            PlayerSkills.Instance.behemothSkillQCoroutine = null;
        }

        if(PlayerSkills.Instance.behemothSkillECoroutine != null)
        {
            StopCoroutine(PlayerSkills.Instance.behemothSkillECoroutine);
            PlayerSkills.Instance.behemothSkillECoroutine = null;
        }
    }
}
