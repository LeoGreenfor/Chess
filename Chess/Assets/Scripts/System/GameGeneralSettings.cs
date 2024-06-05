using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameGeneralSettings", menuName = "ScriptableObjects/GameGeneralSettings", order = 0)]
public class GameGeneralSettings : ScriptableObject
{
    public string SaveGameFilePath;
    public ChessPiecesPrefabs[] chessPiecesPrefabs;

    [System.Serializable]
    public class ChessPiecesPrefabs
    {
        public ChessPiece[] chessPieces;
    }
}
