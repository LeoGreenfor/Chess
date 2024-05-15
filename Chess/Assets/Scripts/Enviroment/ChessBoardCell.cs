using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardCell : MonoBehaviour
{
    [Header("Coordinates")]
    [SerializeField] private char CooLetter;
    [SerializeField] private int CooNumber;

    public string CellCoordinates()
    {
        return CooLetter + CooNumber.ToString();
    }
}
