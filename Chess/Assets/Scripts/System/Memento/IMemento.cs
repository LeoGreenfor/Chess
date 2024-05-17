using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMemento
{
    string GetState();

    DateTime GetDate();
}
