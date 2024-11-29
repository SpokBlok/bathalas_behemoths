using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MarkupoTimer : MonoBehaviour
{
    public float timerDuration = 240f; 
    private float timer;

    public TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = timerDuration;
        UpdateTimerText();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime; // Decrease timer by elapsed time
            UpdateTimerText();
        }
        else
        {
            timer = 0; // Clamp the timer at 0
            UpdateTimerText();
            OnTimerEnd();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:ss
    }

    void OnTimerEnd()
    {
        //Call ending cutscene
        Debug.Log("Timer ended!");
    }
}
