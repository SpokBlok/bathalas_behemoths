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
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        // Call the UI to be activated when paused
        if(pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
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
