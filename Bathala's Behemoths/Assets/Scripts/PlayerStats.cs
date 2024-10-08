using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static PlayerStats Instance { get; private set; }

    public int speed;
    public int basicAttackDamage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
            InitializePlayerStats();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void InitializePlayerStats()
    {
        // Set default values
        speed = 10;
        basicAttackDamage = 25;
    }

    public void AddBasicAttackDamage(int damageIncrease)
    {
        basicAttackDamage += damageIncrease;
    }
}
