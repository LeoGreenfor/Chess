using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStartTrigger : MonoBehaviour
{
    public Action OnBeginCooldown;
    public Action<MatchStartTrigger> OnTriggerActive;
    [SerializeField] private float delaySeconds;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.OnMatchStart?.Invoke();
        OnTriggerActive?.Invoke(this);

        OnBeginCooldown += BeginCooldown;
    }

    private void BeginCooldown()
    {
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        Debug.LogError("a");

        yield return new WaitForSeconds(delaySeconds);

        Debug.LogError("b");
    }

}
