using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDClock : MonoBehaviour
{
    public Image img;
    public Coroutine coolingDown;
    public bool isBasicAttacking = false;
    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
        
        if(player.attackOngoing)
        {
            CoolDownStart();
        }
    }

    public void CoolDownStart()
    {
        if((coolingDown == null && !isBasicAttacking) && !QuestState.Instance.pausedForDialogue)
        {
            coolingDown = StartCoroutine(CoolDown());
            player.attackOngoing = false;
        }
        else if((coolingDown != null && !isBasicAttacking) && !QuestState.Instance.pausedForDialogue)
        {
            StopCoroutine(coolingDown);
            coolingDown = StartCoroutine(CoolDown());
            player.attackOngoing = false;
        }
    }

    IEnumerator CoolDown()
    {
        Debug.Log("Basic Attack Cooling Down");
        isBasicAttacking = true;

        float duration = 1.0f;
        float elapsedTime = 0f;
        float startValue = 1.0f;
        float endValue = 0.0f;
        img.fillAmount = startValue;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            img.fillAmount = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }
        
        img.fillAmount = endValue; // Ensure it fully reaches the target
        isBasicAttacking = false;
    }
}
