using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBoardCell : MonoBehaviour
{
    [Header("Coordinates")]
    [SerializeField] private char CooLetter;
    [SerializeField] private int CooNumber;

    [SerializeField] private Outline _objOutline;

    private ChessBoardHandler _board;

    private void Start()
    {
        _board = FindFirstObjectByType<ChessBoardHandler>();
    }

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

    public void OnMouseExit()
    {
        _objOutline.enabled = false;
        _objOutline.OutlineWidth = 0;
    }

    public void OnMouseEnter()
    {
        _objOutline.enabled = true;
        _objOutline.OutlineWidth = 5;
    }

    private void OnMouseUp()
    {
        _board.OnPlayerMove?.Invoke();
        var player = GameManager.Instance.GetChessPlayer();
        Vector3 towards = gameObject.transform.position;
        player.transform.position = towards;
        //player.GetComponent<CharacterController>().Move(gameObject.transform.position);
    }
}
