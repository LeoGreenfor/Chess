using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLaptopController : MenuController
{
    public FakeTerminal terminal;
    public float delaySeconds;
    private bool _isInNewGameMode;

    private void OnMouseDown()
    {
        terminal.TurnOn(true);
    }

    public void StartNewGame()
    {
        _isInNewGameMode = true;
    }

    public void ChooseSide()
    {
        if (_isInNewGameMode)
        {
            terminal.SetIsCanEnterCommand(true, "ChooseSide");
            GameManager.Instance.SetPlayerSide(terminal.GetInputFromTerminal().Replace("/", ""));
            Debug.LogError(terminal.GetInputFromTerminal().Replace("/", "")); 
            StartCoroutine(LoadingCooldown(2));
        }
        else terminal.SetIsCanEnterCommand(false, "ChooseSide");
        
        //StartCoroutine(LoadingCooldown(1));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadingCooldown(2));
    }

    private IEnumerator LoadingCooldown(int sceneIndex)
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(sceneIndex);
    }
}
