using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State
{
    public override void Start()
    {
        base.Start();
        _character.PopPathfindingTileCell();
    }

    public override void Stop()
    {
        base.Stop();
        _character.ClearPathfindingTileCell();
    }

    public override void Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return;
        }

        if(false == _character.IsEmptyPathfindingTileCell())
        {
            TileCell tileCell = _character.PopPathfindingTileCell();
            _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }
}
