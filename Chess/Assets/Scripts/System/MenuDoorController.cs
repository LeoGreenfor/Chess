using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDoorController : MenuController
{
    private void OnMouseDown() => Application.Quit();
}
