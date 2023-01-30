using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
public class BotController : MonoBehaviour
{
    [SerializeField] private Direction _diretionMove;
    private float _timerMove;
    public bool _isMove;
    public bool _isDead;
    private MeshRenderer _meshRenderer;

    private Vector3 _positionStart;
    private Vector3 _positionMove;
    private Vector3 _directionMove;

    private float _speedMove;

    public Action<BotController> OnDead;

    private void Awake()
    {
        SearchMove();
        _isMove = true;
        _meshRenderer = GetComponent<MeshRenderer>();
        _isDead = false;
        _speedMove = UnityEngine.Random.Range(5, 10) / 2f;
    }

    private void Update()
    {
        if(_isMove)
        {
            Move();
        }
    }

    private void SearchMove()
    {
        Vector3 directionMove = new Vector3(UnityEngine.Random.Range(-2.0f, 2.0f), transform.position.y, UnityEngine.Random.Range(-2.0f, 2.0f));
        //_positionStart = transform.position;
        //int directionMove = UnityEngine.Random.Range(0, 4);
        //switch (directionMove)
        //{
        //    case 0: {
        //            _positionMove = _positionStart + Vector3.right * 1.5f;
        //            break; }
        //    case 1: {
        //            _positionMove = _positionStart + Vector3.left * 1.5f;
        //            break; }
        //    case 2: {
        //            _positionMove = _positionStart + Vector3.forward * 1.5f;
        //            break; }
        //    case 3: {
        //            _positionMove = _positionStart + Vector3.back * 1.5f;
        //            break; }
        //}
        _directionMove = (transform.position - (transform.position + directionMove)).normalized;
        
        _isMove = true;
    }

    private void Move()
    {
        _timerMove += Time.deltaTime;
        if(_timerMove > 1)
        {
            SearchMove();
            _timerMove = 0;
            //transform.position = Vector3.Lerp(_positionStart, _positionMove, _timerMove);
        }
        Vector3 positionNext = transform.position + (_directionMove * _speedMove) * Time.deltaTime;
        positionNext.y = transform.position.y;
        transform.position = positionNext;
        
    }
        
    private void OnMouseDown()
    {
        InfectionBot();
        gameObject.AddComponent<RootsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.CompareTag("Root"))
        {
            if (Vector3.Distance(other.transform.position, transform.position) < 1f)
            {
                if (_isDead != true)
                {
                    InfectionBot();
                    other.GetComponent<RootTile>().InficationBot(transform.position);
                    
                }
            }
            else
            {
                ChangeDirection(other.transform.position);
            }

            
                
        }
    }

    public void Dead()
    {
        OnDead?.Invoke(this);
        _isDead = true;
    }

    private void InfectionBot()
    {
        _isMove = false;
        Dead();
        this.name = "Infection";
        _meshRenderer.material.color = Color.red;
    }

    private void ChangeDirection(Vector3 positionRoot)
    {
        _directionMove = (transform.position - positionRoot).normalized;
        _timerMove = 0;
    }
}

