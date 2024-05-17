using Plugins.MissionCore.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private string _state;
    public ChessSide _playerSide {  get; set; }
    public Player _player { get; set; }
    public int _lastLevelNumber {  get; set; }

    public void SetPlayerSide(string playerSide)
    {
        if (playerSide.ToLower() == ChessSide.White.ToString().ToLower())
        {
            _playerSide = ChessSide.White;
            return;
        }
        if (playerSide.ToLower() == ChessSide.Black.ToString().ToLower())
        {
            _playerSide = ChessSide.Black;
            return;
        }

        Debug.LogError("wrong chess side");
    }

    #region Memento 
    // Saves the current state inside a memento.
    public IMemento Save()
    {
        return new Memento(this._state, this._playerSide, this._player, this._lastLevelNumber);
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
        private ChessSide _playerSide;
        private Player _player;
        private int _lastLevelNumber;

        public Memento(string state, ChessSide chessSide, Player player, int lastLevel)
        {
            _state = state;
            _playerSide = chessSide;
            _player = player;
            _lastLevelNumber = lastLevel;
        }

        public int GetLastLevelNumber()
        {
            return this._lastLevelNumber;
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public ChessSide GetPlayerChessSide()
        {
            return _playerSide;
        }

        public string GetState()
        {
            return _state;
        }
    }
    #endregion
}

public enum ChessSide
{
    White,
    Black
}
