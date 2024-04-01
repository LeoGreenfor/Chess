using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Scene[] scenes;
    [SerializeField] private Button[] mapLevelButton;

    private void Start()
    {
        for (int i = 0; i < scenes.Length; i++)
            mapLevelButton[i].onClick.AddListener(() => LoadLevel(scenes[i]));
    }

    private void LoadLevel(Scene scene)
    {
        SceneManager.LoadScene(scene.buildIndex);
    }
}
