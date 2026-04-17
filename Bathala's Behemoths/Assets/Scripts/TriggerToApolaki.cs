using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToApolaki : MonoBehaviour
{
    public GameObject apolakiHPBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(apolakiHPBar != null)
            {
                apolakiHPBar.SetActive(true);
            }
            

            PlayerStats.Instance.apolakiScene = true;
            PlayerStats.Instance.outdoorsScene = false;
            PlayerStats.Instance.ruinsScene = false;
            PlayerStats.Instance.dead = false;
            PlayerStats.Instance.speedMultiplier = 1.5f;
            SceneManager.LoadScene("ApolakiIntroScene");
            PlayerStats.Instance.SetScenePosition();

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
}
