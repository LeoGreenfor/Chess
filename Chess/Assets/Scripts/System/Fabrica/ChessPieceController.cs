using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceController : EntityController
{
    public override void CreateEntity(Player player)
    {
        var chessPiece = Entity as ChessPiece;
        chessPiece.SetPlayer(player);
    }

    protected override void Retreat()
    {
        throw new System.NotImplementedException();
    }
}
