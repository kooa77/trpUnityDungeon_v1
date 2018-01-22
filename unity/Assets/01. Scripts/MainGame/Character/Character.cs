using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}

public class Character : MapObject
{
    protected GameObject _chracterView;

    protected int _tileX = 0;
    protected int _tileY = 0;

    protected bool _isLive = true;
    protected int _hp = 100;
    protected int _attackPoint = 10;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateAttackCooltime();
        _state.Update();

        // UI
        UpdateUI();
    }


    public void Init(string viewName)
    {
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _chracterView = GameObject.Instantiate(characterViewPrefabs);
        _chracterView.transform.SetParent(transform);
        _chracterView.transform.localPosition = Vector3.zero;
        _chracterView.transform.localScale = Vector3.one;

        TileMap map = GameManager.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        SetCanMove(false);

        InitState();
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

    public bool IsLive() { return _isLive; }

    public int GetHP()
    {
        return _hp;
    }

    public float GetDeltaAttackCooltime()
    {
        return _deltaAttackCooltime;
    }
    
    public float GetAttackCooltime()
    {
        return _attackCooltime;
    }

    int _damagePoint = 0;
    public int GetDamagePoint() { return _damagePoint; }

    eMoveDirection _nextDirection = eMoveDirection.NONE;

    public eMoveDirection GetNextDirection()
    {
        return _nextDirection;
    }

    public void SetNextDirection(eMoveDirection direction)
    {
        _nextDirection = direction;
    }

    // State

    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;

    public virtual void InitState()
    {
        {
            State state = new IdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new MoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new AttackState();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new DamageState();
            state.Init(this);
            _stateMap[eStateType.DAMAGE] = state;
        }
        {
            State state = new DeathState();
            state.Init(this);
            _stateMap[eStateType.DEATH] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }

    public void ChangeState(eStateType nextState)
    {
        if(null != _state)
            _state.Stop();
        _state = _stateMap[nextState];
        _state.Start();
    }


    // Message

    override public void ReceiveObjectMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "Attack":
                //Damaged(msgParam.attackPoint);
                _damagePoint = msgParam.attackPoint;
                _state.NextState(eStateType.DAMAGE);
                break;
        }
    }


    // Actions

    public bool MoveStart(int tileX, int tileY)
    {
        string animationTrigger = "down";

        switch (_nextDirection)
        {
            case eMoveDirection.LEFT:
                animationTrigger = "left";
                break;
            case eMoveDirection.RIGHT:
                animationTrigger = "right";
                break;
            case eMoveDirection.UP:
                animationTrigger = "up";
                break;
            case eMoveDirection.DOWN:
                animationTrigger = "down";
                break;
        }

        _chracterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(tileX, tileY);
        if (0 == collisionList.Count)
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = tileX;
            _tileY = tileY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

            return true;
        }
        return false;
    }

    public void Attack(MapObject enemy)
    {
        ResetCooltime();

        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.message = "Attack";
        msgParam.attackPoint = _attackPoint;

        MessageSystem.Instance.Send(msgParam);
    }

    public void DecreaseHP(int damagePoint)
    {
        _chracterView.GetComponent<SpriteRenderer>().color =
            Color.red;
        Invoke("ResetColor", 0.1f);

        _hp -= damagePoint;
        if (_hp < 0)
        {
            _hp = 0;
            _isLive = false;
        }
    }

    void ResetColor()
    {
        _chracterView.GetComponent<SpriteRenderer>().color =
            Color.white;
    }


    // Cooltime

    float _attackCooltime = 1.0f;
    float _deltaAttackCooltime = 0.0f;

    void UpdateAttackCooltime()
    {
        if (_attackCooltime <= _deltaAttackCooltime)
            _deltaAttackCooltime = _attackCooltime;
        _deltaAttackCooltime += Time.deltaTime;
    }

    void ResetCooltime()
    {
        _deltaAttackCooltime = 0.0f;
    }

    public bool IsAttackable()
    {
        if (_attackCooltime <= _deltaAttackCooltime)
            return true;
        return false;
    }


    // UI

    List<GameSlider> _gaugeList = new List<GameSlider>();

    void UpdateUI()
    {
        for(int i=0; i< _gaugeList.Count; i++)
        {
            _gaugeList[i].Update();
        }
    }

    public void LinkGameGuage(Slider guage, Vector3 position, bool isHPGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        guage.transform.SetParent(canvasObject.transform);
        guage.transform.localPosition = position;
        guage.transform.localScale = Vector3.one;

        GameSlider slider = null;
        if( isHPGuage )
        {
            slider = new HPSlider();
            slider.Init(guage, this);
        }
        else
        {
            slider = new CooltimeSlider();
            slider.Init(guage, this);
        }
        _gaugeList.Add(slider);
    }
}
