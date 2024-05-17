using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemento
{
    string GetState();
    ChessSide GetPlayerChessSide();
    int GetLastLevelNumber();
    Player GetPlayer();
}
