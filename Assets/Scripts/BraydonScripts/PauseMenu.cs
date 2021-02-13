using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public static bool isPaused = false;

    public static bool isSettings = false;
    public static bool isGameOver = false;

    public void Update() 
    {  
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isSettings && !isGameOver) isPaused = !isPaused;
        }
        if (isPaused) ActiveMenu();
        else DeactivateMenu();
        if (isGameOver) GameOverMenu();
        else noGameOverMenu();
        
    }
    public void ActiveMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }
    public void DeactivateMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        // #if UNITY_EDITOR
        //         UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }

    public void OpenSettings() {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        isSettings = true;
    }

    public void GameOverMenu() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void noGameOverMenu() {
        
    }

}
