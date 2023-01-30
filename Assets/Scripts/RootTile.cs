using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTile : MonoBehaviour
{
    private Vector3 _positionStart;
    private Vector3 _positionMove;
    private Transform _targetBot;
    private float _timer;
    private bool _isMove;
    private float _distance;
    private Vector3 _direction;

    public Action<Vector3, RootTile> OnEndGrowth;
    public Action<Vector3, RootTile> OnInfectionBot;

    public void MoveTarget(Vector3 positionStart,BotController informationBots)
    {
        _positionStart = positionStart;
        _positionMove = informationBots.transform.position;
        _direction = (_positionStart - _positionMove).normalized;
        _positionMove = _positionStart - _direction;
        _isMove = true;
        transform.LookAt(_positionStart + _direction);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y - 90, transform.localEulerAngles.z);
    }

    private void Update()
    {
        if (_isMove)
        {
            _timer += Time.deltaTime * 3f;
            if (_timer < 1f)
            {
                transform.position = Vector3.Lerp(_positionStart, _positionMove, _timer);
                transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(2, 1, 1), _timer);
            }
            else
            {
                _isMove = false;
                OnEndGrowth?.Invoke(_positionStart - _direction * 2,this);
            }
        }
    }

    public void InficationBot(Vector3 positionInfication) =>
        OnInfectionBot?.Invoke(positionInfication, this);
    
}
