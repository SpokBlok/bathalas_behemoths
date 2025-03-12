using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillQChargeUpdate : MonoBehaviour
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
        if(PlayerSkills.Instance.behemothSkillQ != null)
        {
            chargesIndicator.text = PlayerSkills.Instance.behemothSkillQCharges.ToString();
        }
    }
}
