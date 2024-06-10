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

        Entity.Create();
    }

    public override void MakeMove(PlayerController player)
    {
        // if player can kill -> retreat
        // else -> attack
        if (Entity.GetAfterAttackHealth(player.Entity.GetStrength()) <= 0f) Entity.Retreat();
        else Entity.Attack(player.Entity);

        base.MakeMove(player);
    }
}
