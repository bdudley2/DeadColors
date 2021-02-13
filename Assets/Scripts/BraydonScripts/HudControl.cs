using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudControl : MonoBehaviour
{
    public static HudControl Instance {get; private set;}

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(gameObject); //use if you want to keep state of hud saved across scenes
        } else {
            Destroy(gameObject);
        }
    }
    // void Awake() {
    //     GameObject.FindWithTag("MainHUD").transform.GetChild(0).GetChild(0).gameObject.tag = "BloodOnScreen";
    // }
    private GameObject controlsHUD;
    private GameObject pauseHUD;
    private GameObject mainHUD;
    private GameObject gameOverHUD;
    private GameObject gameWonHUD;

    private GameObject pauseMenuUI;
    private GameObject settingsMenuUI;
    private GameObject gameOverMenuUI;
    private GameObject gameWonMenuUI;
    private GameObject mainUI;
    private GameObject toolTipHUD;
    public static bool isPaused = false;

    public static bool isSettings = false;
    public static bool isGameOver = false;
    public static bool gameWon = false;

    public static bool gameStarted = false;
    private int numSpaces = 0;

    // Start is called before the first frame update
    void Start()
    {
        controlsHUD = GameObject.FindWithTag("ControlHUD");
        pauseHUD = GameObject.FindWithTag("PauseHUD");
        mainHUD = GameObject.FindWithTag("MainHUD");
        gameOverHUD = GameObject.FindWithTag("GameOverHUD");
        gameWonHUD = GameObject.FindWithTag("GameWonHUD");
        toolTipHUD = GameObject.FindWithTag("ToolTipHUD");
        pauseMenuUI = pauseHUD.transform.GetChild(0).gameObject; //0
        settingsMenuUI = pauseHUD.transform.GetChild(1).gameObject; //1
        gameOverMenuUI = gameOverHUD.transform.GetChild(0).gameObject; //0
        gameWonMenuUI = gameWonHUD.transform.GetChild(0).gameObject; //0
        mainUI = mainHUD.transform.GetChild(0).gameObject; //0
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        gameWonMenuUI.SetActive(false);
        mainHUD.SetActive(true);   // SET TRUE
        // GameObject.FindWithTag("MainHUD").transform.GetChild(0).GetChild(0).gameObject.tag = "BloodOnScreen";
        isPaused = false;
        isSettings = false;
        isGameOver = false;
        gameWon = false;

        // gameStarted = false;
        numSpaces = 0;
        if (gameStarted == true) {
            numSpaces = 3;
        } else {
            GameObject tip1 = toolTipHUD.transform.GetChild(0).GetChild(0).gameObject;
            tip1.SetActive(true);
        }
    }

    public void Update() 
    {  
        if (gameStarted == false) {
            Time.timeScale = 0;
            AudioListener.pause = true;
            GameIntro();
        } else {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (!isSettings && !isGameOver && !gameWon) isPaused = !isPaused;
            }
            if (isPaused) ActiveMenu();
            else DeactivateMenu();
            if (isGameOver && !gameWon) OpenGameOverMenu();
            if (!isGameOver && gameWon) OpenGameWonMenu();
            // else CloseGameOverMenu();
            // if (isSettings && isPaused) OpenSettings();
            // else CloseSettings();
        }
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
    public void CloseSettings() {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        isSettings = false;
    }

    public void OpenGameOverMenu() {
        gameOverMenuUI.SetActive(true);   // SET TRUE
        mainHUD.SetActive(false);
        controlsHUD.transform.GetChild(0).gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        isGameOver = true;
    }

    // not used?
    public void CloseGameOverMenu() {
        gameOverMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;        
        AudioListener.pause = false;
        isGameOver = false;
    }

    public void OpenGameWonMenu() {
        gameWonMenuUI.SetActive(true);   // SET TRUE
        mainHUD.SetActive(false);
        controlsHUD.transform.GetChild(0).gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameWon = true;
    }

    // not used?
    public void CloseGameWonMenu() {
        gameWonMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;        
        AudioListener.pause = false;
        gameWon = false;
    }

    public void GameIntro() {
        if (Input.GetKeyDown("space")) {
            numSpaces++;
            GameObject tip1 = toolTipHUD.transform.GetChild(0).GetChild(numSpaces - 1).gameObject;
            tip1.SetActive(false);
            if (numSpaces < 4) {
                GameObject tipb = toolTipHUD.transform.GetChild(0).GetChild(numSpaces).gameObject;
                tipb.SetActive(true);
            }
        }
        if (numSpaces == 4) {
            Time.timeScale = 1;
            gameStarted = true;
        }
    }
}
