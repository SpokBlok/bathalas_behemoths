using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    //For checking if player is within all enemies ranges
    public static event Action OnDashComplete;

    //For the passive health skill
    public static event Action OnTakingDamage;
    public static event Action OnFullHealth;

    //For enabling/disabling the hp and currency HUDs when entering building upgrades
    public static event Action OnEnteringUpgradeScreen;
    public static event Action OnExitingUpgradeScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void InvokeOnDashComplete()
    {
        OnDashComplete?.Invoke();
    }

    public void InvokeOnTakingDamage()
    {
        OnTakingDamage?.Invoke();
    }

    public void InvokeOnFullHealth()
    {
        OnFullHealth?.Invoke();
    }

    public void InvokeOnEnteringUpgradeScreen()
    {
        OnEnteringUpgradeScreen?.Invoke();
    }

    public void InvokeOnExitingUpgradeScreen()
    {
        OnExitingUpgradeScreen?.Invoke();
    }
}
