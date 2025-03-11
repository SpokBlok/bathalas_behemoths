using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDClock : MonoBehaviour
{
    public Image img;
    public Coroutine coolingDown;

    // Start is called before the first frame update
    void Start()
    {
        img.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoolDownStart()
    {
        if(coolingDown == null && !QuestState.Instance.pausedForDialogue)
        {
            coolingDown = StartCoroutine(CoolDown());
        }
        else if(coolingDown != null && !QuestState.Instance.pausedForDialogue)
        {
            StopCoroutine(coolingDown);
            coolingDown = StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        Debug.Log("Basic Attack Cooling Down");

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
    }
}
