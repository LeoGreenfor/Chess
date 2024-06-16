using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndController : MonoBehaviour
{
    [SerializeField] private int neededWinsCount;
    private void OnTriggerEnter(Collider other)
    {
        int winsCount = GameManager.Instance.LevelHandler.WinsCounts;

        if (winsCount == neededWinsCount) GameManager.Instance.OnEnterNextLevel?.Invoke();
    }
}
