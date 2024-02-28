using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused;
    //[SerializeField] private UIController UIController;

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
        //UIController.pause.SetActive(true);
        //UIController.letterBackground.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        //UIController.pause.SetActive(false);
        //UIController.letterBackground.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}