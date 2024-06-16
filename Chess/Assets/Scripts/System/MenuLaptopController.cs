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
        terminal.gameObject.SetActive(true);
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

            var player = new PlayerData();
            GameManager.Instance.LastLevelNumber = 1;
            GameManager.Instance.SaveGame(player);

            StartCoroutine(LoadingCooldown(GameManager.Instance.LastLevelNumber));
        }
        else terminal.SetIsCanEnterCommand(false, "ChooseSide");
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        StartCoroutine(LoadingCooldown(GameManager.Instance.LastLevelNumber));
    }

    private IEnumerator LoadingCooldown(int sceneIndex)
    {
        yield return new WaitForSeconds(delaySeconds);
        _eventCanvas.SetActive(true);
        SceneManager.LoadScene(sceneIndex);
    }
}
