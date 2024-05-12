using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuController : MonoBehaviour
{
    [SerializeField] private Outline _objOutline;
    [SerializeField] private GameObject _eventCanvas;

    public void OnMouseExit()
    {
        _objOutline.enabled = false;
        _objOutline.OutlineWidth = 0;
        Debug.LogWarning("exit obj");
    }

    public void OnMouseEnter()
    {
        Debug.LogWarning("enter obj");
        _objOutline.enabled = true;
        _objOutline.OutlineWidth = 5;
    }
}
