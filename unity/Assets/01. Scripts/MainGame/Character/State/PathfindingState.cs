using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    enum eUpdateState
    {
        PATHFINDING,
        BUILD_PATH,
    }
    eUpdateState _updateState = eUpdateState.PATHFINDING;

    struct sPosition
    {
        public int x;
        public int y;
    }

    struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }
    //Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();
    List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();

    TileCell _goalTileCell;
    TileCell _reverseTileCell = null;

    override public void Start()
    {
        base.Start();

        _goalTileCell = _character.GetGoalTileCell();
        if(null != _goalTileCell)
        {
            GameManager.Instance.GetMap().ResetPathfinding();

            TileCell startTileCell =
                GameManager.Instance.GetMap().GetTileCell(_character.GetTileX(),
                                                    _character.GetTileY());

            sPathCommand command;
            command.tileCell = startTileCell;
            command.heuristic = 0;
            //_pathfindingQueue.Enqueue(command);
            //_pathfindingQueue.Add(command);
            PushCommand(command);
        }
        else
        {
            _nextState = eStateType.IDLE;
        }

        _reverseTileCell = null;
        _updateState = eUpdateState.PATHFINDING;
    }

    public override void Stop()
    {
        base.Stop();

        _pathfindingQueue.Clear();
        _character.SetGoalTileCell(null);
    }

    override public void Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return;
        }

        switch(_updateState)
        {
            case eUpdateState.PATHFINDING:
                UpdatePathfinding();
                break;
            case eUpdateState.BUILD_PATH:
                UpdateBuildPath();
                break;
        }
    }

    void UpdatePathfinding()
    {
        // 길찾기 알고리즘이 시작
        if (0 != _pathfindingQueue.Count)
        {
            //sPathCommand command = _pathfindingQueue.Dequeue();
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);
            if (false == command.tileCell.IsPathfided())
            {
                command.tileCell.Pathfinded();

                // 목표에 도달 했나?
                if (command.tileCell.GetTileX() == _goalTileCell.GetTileX() &&
                    command.tileCell.GetTileY() == _goalTileCell.GetTileY())
                {
                    Debug.Log("Finded");
                    _reverseTileCell = command.tileCell;
                    _updateState = eUpdateState.BUILD_PATH;
                    return;
                }

                for (int direction = (int)eMoveDirection.LEFT;
                    direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GetPositionByDirection(curPosition, (eMoveDirection)direction);

                    TileCell searchTileCell =
                        GameManager.Instance.GetMap().GetTileCell(nextPosition.x, nextPosition.y);
                    if (searchTileCell.CanMove() && false == searchTileCell.IsPathfided())
                    {
                        float distance = command.tileCell.GetDistanceFromStart() +
                            searchTileCell.GetDistanceWeight();

                        if(null == searchTileCell.GetPrevPathfindingCell())
                        {
                            searchTileCell.SetDistanceFromStart(distance);
                            searchTileCell.SetPrevPathfindingCell(command.tileCell);
                            searchTileCell.SetPathfindingTestMark();

                            sPathCommand newCommand;
                            newCommand.tileCell = searchTileCell;
                            newCommand.heuristic = distance;
                            //_pathfindingQueue.Enqueue(newCommand);
                            //_pathfindingQueue.Add(newCommand);
                            PushCommand(newCommand);
                        }                
                        else
                        {
                            if(distance < searchTileCell.GetDistanceFromStart())
                            {
                                searchTileCell.SetDistanceFromStart(distance);
                                searchTileCell.SetPrevPathfindingCell(command.tileCell);

                                sPathCommand newCommand;
                                newCommand.tileCell = searchTileCell;
                                newCommand.heuristic = distance;
                                //_pathfindingQueue.Enqueue(newCommand);
                                //_pathfindingQueue.Add(newCommand);
                                PushCommand(newCommand);
                            }
                        }
                    }
                }
            }
        }
    }

    void UpdateBuildPath()
    {
        if(null != _reverseTileCell)
        {
            _character.PushPathfindingTileCell(_reverseTileCell);

            _reverseTileCell.ResetPathfindingTestMark();
            _reverseTileCell = _reverseTileCell.GetPrevPathfindingCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }

    sPosition GetPositionByDirection(sPosition curPosition, eMoveDirection direction)
    {
        sPosition newPosition = curPosition;
        switch (direction)
        {
            case eMoveDirection.LEFT:
                newPosition.x--;
                break;
            case eMoveDirection.RIGHT:
                newPosition.x++;
                break;
            case eMoveDirection.UP:
                newPosition.y--;
                break;
            case eMoveDirection.DOWN:
                newPosition.y++;
                break;
        }
        return newPosition;
    }

    void PushCommand(sPathCommand command)
    {
        _pathfindingQueue.Add(command);
        // Sorting
    }
}
