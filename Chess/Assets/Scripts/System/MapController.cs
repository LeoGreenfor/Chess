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

    public void LoadLevel(SceneAsset scene)
    {
        Debug.LogError("a");
        SceneManager.LoadScene(scene.name);
    }
}
