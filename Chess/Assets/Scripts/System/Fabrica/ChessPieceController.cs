using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceController : EntityController
{
    public override EntityController SpawnEntity(ChessBoardCell cell)
    {
        this.Entity = GetComponent<ChessPiece>();
        Debug.LogError(this.Entity);

        return base.SpawnEntity(cell);
    }

    public override void CreateEntity(Player player)
    {
        base.CreateEntity(player);
        gameObject.GetComponent<ChessPiece>().SetPlayer(player);
    }

    public override void MakeMove(PlayerController player)
    {
        Debug.LogError(this.Entity);
        Debug.LogError($"entity {this.Entity is null}, entity as chess piece {(this.Entity as ChessPiece) is null}");

        // if player can kill -> retreat
        // else -> attack
        if (player.IsCorrectCoordinates(CurrentCell) && (Entity.GetAfterAttackHealth(player.Entity.GetStrength()) <= 0f)) Entity.Retreat();
        else Entity.Attack(player.CurrentCell);

        base.MakeMove(player);
    }
}
