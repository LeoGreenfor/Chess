using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLaptopController : MenuController
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}