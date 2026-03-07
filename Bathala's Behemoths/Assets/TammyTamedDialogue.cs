using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TammyTamedDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] currentLines;
    public float textInterval;
    public GameObject pointer;
    public GameObject HUD;

    private bool pointerActive = false;
    Vector3 currentPosition;
    Vector3 originalHUDPos;
    private int index;
    public BathalasBlessing bbSkill;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        currentPosition = gameObject.transform.localPosition;
    }

    void OnEnable()
    {
        PlayerStats.Instance.currentHealth = PlayerStats.Instance.maxHealth;
        HUD = GameObject.FindGameObjectWithTag("HUD");
        originalHUDPos = HUD.gameObject.transform.position;
        HUD.gameObject.transform.position = new Vector3(10000, 10000, 10000);
        QuestState.Instance.pausedForDialogue = true;
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textInterval);
        }
    }

    void NextLine()
    {
        if(index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if(pointerActive == true)
        {
            gameObject.transform.localPosition = currentPosition;
            QuestState.Instance.pausedForDialogue = false;
            HUD.gameObject.transform.position = originalHUDPos;
            GoToBase();
            
            Debug.Log("inside end state");
        }
        else
        {
            gameObject.transform.localPosition = new Vector3 (1000, 1000);
            pointer.SetActive(true);
            pointerActive = true;
        }
    }

    void GoToBase()
    {
        SceneManager.LoadScene("RuinsScene Movement");
        PlayerStats.Instance.introDone = true;
        PlayerStats.Instance.outdoorsScene = false;
        PlayerStats.Instance.ruinsScene = true;
        PlayerStats.Instance.tammyScene = false;
        PlayerStats.Instance.markyScene = false;
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
