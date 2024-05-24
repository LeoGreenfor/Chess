using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardCell : MonoBehaviour
{
    [Header("Coordinates")]
    [SerializeField] private char CooLetter;
    [SerializeField] private int CooNumber;

    public string CellOriginalCoordinates()
    {
        return CooLetter + CooNumber.ToString();
    }

    public int[] CellToIntCoordinates()
    {
        int baseAsciiValue = (int)'A' - 1;

        int CooLetterToInt = (int)CooLetter - baseAsciiValue;

        return new int[2] { CooLetterToInt, CooNumber };
    }
}
