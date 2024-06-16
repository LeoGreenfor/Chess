using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : EntityController
{
    public override EntityController SpawnEntity(ChessBoardCell cell)
    {
        Entity = GetComponent<Player>();
        Debug.LogError(Entity);

        return base.SpawnEntity(cell);
    }
    public override void CreateEntity(Player player)
    {
        base.CreateEntity(player);

        int playerLevel = player.Level;

        if (playerLevel == 1)
        {
            player.SetStats(1);
            goToX = 1;
            goToY = 1;
            isMoveByStraight = true;
            isMoveByDiagonal = false;
        }
        if (playerLevel == 2)
        {
            player.SetStats(2);
            goToX = 4;
            goToY = 4;
            isMoveByStraight = true;
            isMoveByDiagonal = false;
        }
        if (playerLevel == 3) 
        {
            player.SetStats(3);
            goToX = 4;
            goToY = 4;
            isMoveByStraight = false;
            isMoveByDiagonal = true;
        }
        if (playerLevel == 4)
        {
            player.SetStats(4);
            goToX = 4;
            goToY = 4;
            isMoveByStraight = true;
            isMoveByDiagonal = true;
        }
    }

    public override void MakeMove(PlayerController player)
    {
        base.MakeMove(player);
    }
}
