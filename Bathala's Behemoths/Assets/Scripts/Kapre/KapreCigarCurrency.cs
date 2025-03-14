using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KapreCigarCurrency : MonoBehaviour
{
    public TextMeshProUGUI CigarText;

    // public static KapreCigarCurrency Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CigarText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CigarText.text = PlayerStats.Instance.kapreCigars.ToString();
    }

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject); // Keep this object across scenes
    //     }
    //     else
    //     {
    //         Destroy(gameObject); // Destroy duplicate instance
    //     }
    // }

    private void EnableUI()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void DisableUI()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
