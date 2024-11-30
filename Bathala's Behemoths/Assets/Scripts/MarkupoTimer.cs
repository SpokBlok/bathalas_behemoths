using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MarkupoTimer : MonoBehaviour
{
    public SpawnerScript spawner;
    public float timerDuration = 240f; 
    private float timer;
    private float spawnTimer = 5f;

    public TextMeshProUGUI timerText;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = timerDuration;
        UpdateTimerText();
    }

    void Update()
    {
        if (timer > 0f)
        {
            if (spawnTimer <= 0f)
            {
                spawner.SpawnEnemy();
                spawner.SpawnEnemy();
                spawnTimer = 5f;
            }
            timer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime; // Decrease timers by elapsed time
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
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:ss
    }

    void OnTimerEnd()
    {
        //Call ending cutscene
        Debug.Log("Timer ended!");
    }
}
