using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Direction
{
    Right = 2,
    Left = -2,
    Up = 1,
    Down = -1,
    Stop
}
public class BotController : MonoBehaviour
{
    //[SerializeField] private Direction _diretionMove;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    private float _timerMove;
    public bool _isMove;
    public bool _isDead;
    private MeshRenderer _meshRenderer;

    private Vector3 _positionStart;
    private Vector3 _positionMove;
    private Vector3 _directionMove;
    private Vector3 _movement;
    private float _speedMove;

    private float _lastDirection;
    private Direction _currentDirection;

    private float _time;
    private float _timerStop;

    public Action<BotController> OnDead;

    private void Awake()
    {
        _lastDirection = 0;
        _time = 3f;
        _timerMove = 0;
        _timerStop = 0;
        SearchMove();
        _isMove = true;
        _meshRenderer = GetComponent<MeshRenderer>();
        _isDead = false;
        _speedMove = UnityEngine.Random.Range(2, 6) / 2f;
    }

    private void Update()
    {
        if(_isMove)
        {
            ChangeDirection();
            Move();


        }
    }

    private void ChangeDirection()
    {
        switch (_currentDirection)
        {
            case Direction.Down:
                {
                    _directionMove = Vector3.back;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
                }
            case Direction.Up:
                {
                    _directionMove = Vector3.forward;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                }
            case Direction.Left:
                {
                    _directionMove = Vector3.left;
                    transform.eulerAngles = new Vector3(0, 270, 0);
                    break;
                }
            case Direction.Right:
                {
                    _directionMove = Vector3.right;
                    transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                }
            case Direction.Stop:
                {
                    SetStateStop();
                    break;
                }
        }
    }

    private void SetStateStop()
    {
        _animator.SetBool("Walk", false);
        _animator.SetBool("Idle", true);
        _directionMove = Vector3.zero;
        if (_timerStop > UnityEngine.Random.Range(1f, 5f))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("Walk", true);
            SearchMove();
            _timerStop = 0;
            
        }
        _timerStop += Time.deltaTime;
    }




    private void SearchMove()
    {
        int direction = UnityEngine.Random.Range(-2, 2);
        _currentDirection = (Direction)direction;
        Debug.Log(_currentDirection);

    }


    private void Move()
    {
        if (_timerMove > _time)
        {
            _currentDirection = Direction.Stop;
            _timerMove = 0;
        }
        else
        {
            transform.position += _directionMove * _speedMove * Time.deltaTime;
            
        }
            
        _timerMove += Time.deltaTime;
    }








    //private void SearchMove()
    //{
    //    //Vector3 directionMove = new Vector3(UnityEngine.Random.Range(-2.0f, 2.0f), transform.position.y, UnityEngine.Random.Range(-2.0f, 2.0f));
    //    //_directionMove = (transform.position - (transform.position + directionMove)).normalized;
    //    float step = UnityEngine.Random.Range(-5.5f, 5.5f);
    //    _currentDirection = UnityEngine.Random.Range(0, 4); // 0 - up, 1 - down, 2 - right, 3 - left
    //    if (_currentDirection != _lastDirection)
    //    {
    //       _movement = ChangeDirection(_currentDirection, step);
    //        _lastDirection = _currentDirection;
    //    }
    //    _directionMove = (transform.position - (transform.position + _movement)).normalized;
    //    _time = UnityEngine.Random.Range(10f, 20f);

    //}

    //private Vector3 ChangeDirection(float direction, float step) {
    //    Vector3 movement = direction switch
    //    {
    //        0 => new Vector3(transform.position.x, transform.position.z, step),
    //        1 => new Vector3(transform.position.x, transform.position.z, -step),
    //        2 => new Vector3(step, transform.position.y, transform.position.z),
    //        3 => new Vector3(-step, transform.position.y, transform.position.z),
    //    } ;
    //    return movement;
    //}

    //private void Move()
    //{
    //    _timerMove += Time.deltaTime;
    //    if(_timerMove > _time)
    //    {
    //        SearchMove();
    //        _timerMove = 0;
    //        //transform.position = Vector3.Lerp(_positionStart, _positionMove, _timerMove);
    //    }
    //    Vector3 positionNext = transform.position + _directionMove * Time.deltaTime;
    //    positionNext.y = transform.position.y;
    //    transform.position = positionNext;

    //}

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

