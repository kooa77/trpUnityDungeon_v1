using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingIdleState : State
{
    override public void Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return;
        }

        TileCell goalTileCell = _character.GetGoalTileCell();
        if(null != goalTileCell)
        {
            _nextState = eStateType.PATHFINDING;
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                MapObject mapObject = hit.collider.gameObject.GetComponent<MapObject>();
                if(null != mapObject)
                {
                    if(eMapObjectType.TILE_OBJECT == mapObject.GetObjectType())
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                        TileCell selectTileCell = GameManager.Instance.GetMap().GetTileCell(mapObject.GetTileX(), mapObject.GetTileY());
                        _character.SetGoalTileCell(selectTileCell);
                    }
                }
            }
        }
    }
}
