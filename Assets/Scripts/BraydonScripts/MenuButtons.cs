using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuButtons : MonoBehaviour
{

    // These are being defined manually as of now
    public string startScene1 = "StartScene";
    public string gameScene1 = "MainScene";


// --------------------------------------------------
// MAIN MENU FUNCTIONS 

    // Main Menu: button to play game is pressed
    public void startGame() {
        SceneManager.LoadScene(gameScene1);
        HudControl.gameStarted = false;
        //PlayerMovement.takingDamage = false;
        PlayerMovement.count = 0; 
        PlayerMovement.c.a = 0;
        if (PlayerMovement.damageGO != null) {
            if (PlayerMovement.damageImage != null) PlayerMovement.damageImage.color = PlayerMovement.c; 
        }
    }

    // Main Menu: button to exit entire game application
    public void exitApplication() {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    // Main Menu: settings button pressed to bring up settings Menu
    public void viewMainMenuSettings() {
        // GetComponent<PauseMenu>().DeactivateMenu();

        // make settings HUD visible here
    }

    // Main Menu: button that exits settings Menu in Main Menu Scene
    public void exitMainMenuSettings() {
        // make settings HUD disappear here
    }


// --------------------------------------------------
// GAME MENU FUNCTIONS 

    // Game Menu: button to quit to Main Menu if pressed
    public void quitToStartScene() {
        // GameObject.FindWithTag("pointSystem").GetComponent<PauseMenu>().isGameOver = false;
        SceneManager.LoadScene(startScene1);
    }

    // Game Menu: button to resume game from a pause menu
    public void resumeGame() {
        // Set Time to be normal and enable/disable the approapriate HUD's
        HudControl.Instance.DeactivateMenu();
    }

    // Game Menu: button to restart game from any game menu (pause/gameover/victory)
    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HudControl.gameStarted = true;
        //PlayerMovement.takingDamage = false;
        PlayerMovement.count = 0; 
        PlayerMovement.c.a = 0;
        if (PlayerMovement.damageGO != null) {
            if (PlayerMovement.damageImage != null) PlayerMovement.damageImage.color = PlayerMovement.c; 
        }
        // GameObject.FindWithTag("pointSystem").GetComponent<PauseMenu>().isGameOver = false;
    }

    // Game Menu: button to bring up Settings Menu from the pause menu
    public void viewGameSettings() {
        // make settings HUD visible here
    }

    // Game Menu: button to exit Settings Menu and go back to the puase menu
    public void exitGameSettings() {
        // make settings HUD disappear here
    }

}
