using LevelGeneratorRelated.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public int LevelNumber;
    public string LevelName;
    public int WinsCounts;

    public ChessBoardHandler ChessBoardHandler;
    public PlayerMovementController PlayerMovementController;

    public MatchStartTrigger[] MatchStartTriggers;
    [SerializeField] private float delaySeconds;
    public LevelEndController LevelEndController;
    public LevelGenerator LevelGenerator;

    public Action OnEndMatch;
    public Action OnLevelLoad;

    private MatchStartTrigger _currentMatchTrigger;

    private void Start()
    {
        OnEndMatch += BeginCooldown;
        ChessBoardHandler.SetLevelName(LevelName);
        LevelGenerator.OnCreatedLevel += SetTriggers;
    }
    private void SetTriggers()
    {
        MatchStartTriggers = GetComponentsInChildren<MatchStartTrigger>();
        LevelEndController = GetComponentInChildren<LevelEndController>();

        for (int i = 0; i < MatchStartTriggers.Length; i++)
        {
            MatchStartTriggers[i].OnTriggerActive += SetCurrentMatchTrigger;
        }
    }

    private void SetCurrentMatchTrigger(MatchStartTrigger matchStartTrigger)
    {
        _currentMatchTrigger = matchStartTrigger;
    }

    private void BeginCooldown()
    {
        var obj = _currentMatchTrigger;
        StartCoroutine(CoolDown(obj));
    }
    private IEnumerator CoolDown(MatchStartTrigger trigger)
    {
        Debug.LogError("a");
        trigger.gameObject.SetActive(false);

        yield return new WaitForSeconds(delaySeconds);

        Debug.LogError("b");
        trigger.gameObject.SetActive(true);
    }
}
