using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool gameIsPaused;
    public Action <bool> OnPause;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _goToMenuButton;
    [SerializeField] private Canvas canvas;

    private CursorLockMode cursorLockMode;
    private bool cursorVisible;

    private void Start()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _goToMenuButton.onClick.AddListener(GoToMenu);

        OnPause += ShowMenuCanvas;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void ShowMenuCanvas(bool state)
    {
        canvas.gameObject.SetActive(state);
    }

    public void PauseGame()
    {
        cursorLockMode = Cursor.lockState;
        cursorVisible = Cursor.visible;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
        OnPause?.Invoke(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        OnPause?.Invoke(false);
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLockMode;
        Time.timeScale = 1;
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}