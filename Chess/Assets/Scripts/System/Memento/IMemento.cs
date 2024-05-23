using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemento
{
    ChessSide GetPlayerChessSide();
    int GetLastLevelNumber();
    Player GetPlayer();
}
