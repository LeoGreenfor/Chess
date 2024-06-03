using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStartTrigger : MonoBehaviour
{
    [SerializeField] private bool isBeenActivated;
    [SerializeField] private float delaySeconds;

    private ChessBoardHandler _boardHandler;

    private void Start()
    {
        _boardHandler = FindFirstObjectByType<ChessBoardHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBeenActivated)
        {
            GameManager.Instance.OnMatchStart?.Invoke();
            isBeenActivated = true;
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        Debug.LogError("a");

        yield return new WaitForSeconds(delaySeconds);

        isBeenActivated = false;
        Debug.LogError("b");
    }

}
