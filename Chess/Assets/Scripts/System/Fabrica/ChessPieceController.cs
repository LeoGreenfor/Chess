using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceController : EntityController
{
    protected override void CreateEntity()
    {
        throw new System.NotImplementedException();
    }

    /*protected override void MoveTo(Transform newTransform)
    {
        transform.position = Vector3.Lerp(transform.position, newTransform.position, Time.deltaTime * moveSpeed);
    }*/

    protected override void Retreat()
    {
        throw new System.NotImplementedException();
    }
}
