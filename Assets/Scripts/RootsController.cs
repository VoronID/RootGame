using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsController : MonoSingleton<RootsController>
{
    [SerializeField] private BotController _targetBot;
    [SerializeField] private List<BotController> _targetBots;
    [SerializeField] private Transform _roots;
    [SerializeField] private List<Root> _rootsSpawn;
    private Vector3 _positionStart;
    private Vector3 _positionMove;
    //private List<InformationBots> _targetBot;
    private float _timer;
    [SerializeField] private int hpRoot = 10000;


    protected void Awake()
    {
        base.Awake();
        _rootsSpawn = new List<Root>();
        _targetBots = new List<BotController>();
        _targetBot = LevelController.Instance.Get—losestBot(transform.position);
        SpawnRoot(transform.position);
    }
    

    private void SpawnRoot(Vector3 positionStart)
    {
        Root root = new Root(_targetBot, positionStart);
        root.OnInficationBot += InficationBot;
        _rootsSpawn.Add(root);
        //RootTile root = LevelController.Instance.GetRoot();
        //root.OnEndGrowth += ContinueGrowth;
        //root.OnInfectionBot += InficationBot;
        //root.transform.position = positionStart;
        //root.MoveTarget(positionStart, _targetBot);
        //_rootsSpawn.Add(root);
    }
    private void SpawnRoot(Vector3 positionStart, BotController target)
    {
        Root root = new Root(target, positionStart);
        root.OnInficationBot += InficationBot;
        _rootsSpawn.Add(root);
    }

    private void InficationBot(Vector3 positionInfication, Root root)
    {
        hpRoot += 20;
        _targetBots.Clear();

        int countRoot = Random.Range(1, 3);
        Debug.Log("Spawn Root:" + countRoot);
        if(countRoot == 1)
        {

            root._targetBot = LevelController.Instance.Get—losestBot(positionInfication);
        }
        if(countRoot == 2)
        { 
            _targetBots = LevelController.Instance.Get—losestBot(2, positionInfication);
            for(int i = 0; i < _targetBots.Count; i++)
            {
                Debug.Log(_targetBots[i].name); 
            }

            root._targetBot = _targetBots[0];
            SpawnRoot(positionInfication, _targetBots[1]);
        }


    }

    

}
