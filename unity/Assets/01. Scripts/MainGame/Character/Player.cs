using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start ()
    {
	}
	
    /*
    void Update ()
    {
        if (false == _isLive)
            return;

        _state.Update();
    }
    */
    /*
    void Move(eMoveDirection moveDirection)
    {
        string animationTrigger = "down";

        int moveX = _tileX;
        int moveY = _tileY;
        switch(moveDirection)
        {
            case eMoveDirection.LEFT:
                moveX--;
                animationTrigger = "left";
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                animationTrigger = "right";
                break;
            case eMoveDirection.UP:
                moveY++;
                animationTrigger = "up";
                break;
            case eMoveDirection.DOWN:
                moveY--;
                animationTrigger = "down";
                break;
        }

        _chracterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        if(0 == collisionList.Count)
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
        else
        {
            for(int i=0; i< collisionList.Count; i++)
            {
                switch(collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        Attack(collisionList[i]);
                        break;
                }
            }
        }
    }
    */
}
