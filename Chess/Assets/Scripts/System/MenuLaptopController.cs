using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLaptopController : MenuController
{
    public FakeTerminal terminal;
    private bool isTerminalTurnOn;

    private void OnMouseDown()
    {
        isTerminalTurnOn = !isTerminalTurnOn;

        terminal.TurnOn(isTerminalTurnOn);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
