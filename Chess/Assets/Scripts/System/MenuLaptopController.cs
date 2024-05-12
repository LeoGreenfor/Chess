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

    private void OnMouseDown()
    {
        terminal.TurnOn(true);
    }

    public void StartNewGame()
    {
        StartCoroutine(LoadingCooldown(1));
    }
    public void LoadGame()
    {
        StartCoroutine(LoadingCooldown(1));
    }

    private IEnumerator LoadingCooldown(int sceneIndex)
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(sceneIndex);
    }
}
