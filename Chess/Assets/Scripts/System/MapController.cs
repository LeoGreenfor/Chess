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
        /*for (int i = 0; i < mapLevelButton.Length; i++)
        {
            Debug.LogError(i);
            mapLevelButton[i].onClick.AddListener(() => LoadLevel(scenes[i]));
        }*/
    }

    public void LoadLevel(SceneAsset scene)
    {
        Debug.LogError("a");
        SceneManager.LoadScene(scene.name);
    }

    private void OnDestroy()
    {
        /*for (int i = 0; i < mapLevelButton.Length; i++)
        {
            Debug.LogError(i);
            mapLevelButton[i].onClick.RemoveListener(() => LoadLevel(scenes[i]));
        }*/
    }
}
