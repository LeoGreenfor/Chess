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
            player.SetStats(10, 5, 5);
            goToX = 1;
            goToY = 1;
            isMoveByStraight = true;
            isMoveByDiagonal = false;
        }
        if (playerLevel == 2)
        {
            player.SetStats(25, 10, 10);
            goToX = 4;
            goToY = 4;
            isMoveByStraight = true;
            isMoveByDiagonal = false;
        }
        if (playerLevel == 3)
        {
            player.SetStats(50, 15, 10);
            goToX = 2;
            goToY = 2;
            isMoveByStraight = true;
            isMoveByDiagonal = false;
        }
        if (playerLevel == 4) 
        {
            player.SetStats(75, 20, 15);
            goToX = 4;
            goToY = 4;
            isMoveByStraight = false;
            isMoveByDiagonal = true;
        }
        if (playerLevel == 5)
        {
            player.SetStats(100, 25, 15);
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
