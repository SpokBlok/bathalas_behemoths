using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    bool isPaused = false;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseOnEscape();
        }
    }

    public void TogglePauseOnEscape()
    {
        if(!QuestState.Instance.menuActive)
        {
            isPaused = !isPaused;
            QuestState.Instance.pauseActive = isPaused;
            QuestState.Instance.pausedForDialogue = !QuestState.Instance.pausedForDialogue;
            Time.timeScale = isPaused ? 0 : 1;

            // Show cursor when menu is open
            UnityEngine.Cursor.visible = isPaused;
            if(isPaused)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }

            // Call the UI to be activated when paused
            if(pauseMenu != null)
            {
                pauseMenu.SetActive(isPaused);
            }
        }
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public void ReturnToTitle()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
