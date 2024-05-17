using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // For the sake of simplicity, the originator's state is stored inside a
    // single variable.
    private string _state;

    // Saves the current state inside a memento.
    public IMemento Save()
    {
        return new Memento(this._state);
    }

    // Restores the Originator's state from a memento object.
    public void Restore(IMemento memento)
    {
        if (!(memento is Memento))
        {
            throw new Exception("Unknown memento class " + memento.ToString());
        }

        this._state = memento.GetState();
        Console.Write($"Originator: My state has changed to: {_state}");
    }

    private class Memento : IMemento
    {
        private string _state;

        private DateTime _date;

        public Memento(string state)
        {
            this._state = state;
            this._date = DateTime.Now;
        }

        // The Originator uses this method when restoring its state.
        public string GetState()
        {
            return this._state;
        }

        public DateTime GetDate()
        {
            return this._date;
        }
    }
}
