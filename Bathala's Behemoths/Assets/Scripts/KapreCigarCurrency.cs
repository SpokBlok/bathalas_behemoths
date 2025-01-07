using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KapreCigarCurrency : MonoBehaviour
{
    public TextMeshProUGUI CigarText;

    // Start is called before the first frame update
    void Start()
    {
        CigarText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CigarText.text = "Kapre Cigars: " + PlayerStats.Instance.kapreCigars;
    }
}
