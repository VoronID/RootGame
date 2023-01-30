using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Root
{
    public BotController _targetBot;
    public Vector3 _position;
    public Action<Vector3, Root> OnInficationBot;
    public Root(BotController targetBot, Vector3 position)
    {
        _targetBot = targetBot;
        _position = position;
       
        SpawnRoot(_position);
    }

    private void ContinueGrowth(Vector3 positionStart, RootTile root)
    {
        if(_targetBot._isDead != true)
         LevelController.Instance.AddFreeBots(_targetBot);

        BotController newTarget = LevelController.Instance.Get—losestBot(positionStart);
        _targetBot = newTarget;
        //if(_targetBot._isDead != true)
        //{
        //    if(_targetBot != newTarget)
        //    {

        //        _targetBot = newTarget;
        //    }
        //    else
        //    {
        //        _targetBot = newTarget;
        //    }
        //}
        //else
        //{
        //    _targetBot = newTarget;
        //}
        SpawnRoot(positionStart);   
    }

    private void InficationBot(Vector3 positionInfication, RootTile root)
    {
        //_targetBot = LevelController.Instance.Get—losestBot(1, positionInfication);
        OnInficationBot?.Invoke(positionInfication, this);
    }

    private void SpawnRoot(Vector3 positionStart)
    {
        RootTile root = LevelController.Instance.GetRoot();
        root.OnEndGrowth += ContinueGrowth;
        root.OnInfectionBot += InficationBot;
        root.transform.position = positionStart;
        root.MoveTarget(positionStart, _targetBot);
    }
}
