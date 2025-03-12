using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeUpdates : MonoBehaviour
{
    public TMP_Text chargesIndicator;

    // Start is called before the first frame update
    void Start()
    {
        chargesIndicator = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSkills.Instance.behemothSkillE != null)
        {
            chargesIndicator.text = PlayerSkills.Instance.behemothSkillECharges.ToString();
        }
    }
}
