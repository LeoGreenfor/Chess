using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private SceneAsset[] scenes;
    [SerializeField] private Button[] mapLevelButton;

    private void Start()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            Debug.LogError("a");
            if (mapLevelButton[i] == null) Debug.LogError("button is null");
            if (scenes[i] == null) Debug.LogError("scene is null");
            //Debug.Log($"{mapLevelButton[i]}, {mapLevelButton[i].onClick}, {scenes[i]}");
            mapLevelButton[i].onClick.AddListener(() => LoadLevel(scenes[i]));
        }
    }

    private void LoadLevel(SceneAsset scene)
    {
        Debug.LogError("a");
        SceneManager.LoadScene(scene.name);
    }
}
