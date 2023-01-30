using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    [SerializeField] private List<BotController> _bots;
    [SerializeField] private List<BotController> _freeBots;
    [SerializeField] private RootTile _prefabRoot;

    private void Awake()
    {
        _freeBots = new List<BotController>();
        for (int i = 0; i < _bots.Count; i++)
        {
            _bots[i].OnDead += OnDeadBot;
            _freeBots.Add(_bots[i]);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _bots.Count; i++)
        {
            _bots[i].OnDead -= OnDeadBot;
        }
    }

    private void OnDeadBot(BotController botController)
    {
        _bots.Remove(botController);
        _freeBots.Remove(botController);
    }

    public void AddFreeBots(BotController botController)
    {
        _freeBots.Add(botController);
    }

    public BotController Get—losestBot(Vector3 position)
    {
        List<BotController> target = new List<BotController>();
        int count = 0;
        Dictionary<int, float> distance = new Dictionary<int, float>();
        for(int i = 0; i < _freeBots.Count; i++)
        {
            BotController bot = _freeBots[i];
                   
            float distanceToBot = Vector3.Distance(position, bot.transform.position);
            distance.Add(i, distanceToBot);
            
        }
        foreach (var item in distance.OrderBy(v => v.Value))
        {
            if(count == 0)
            {
                
                target.Add(_freeBots[item.Key]);
                _freeBots.RemoveAt(item.Key);
                count++;
            }
            // items are in sorted order
            
            
        }

        return target[0];
    }
    public List<BotController> Get—losestBot(int countBotsGets, Vector3 position)
    {
        List<BotController> target = new List<BotController>();
        int count = 0;
        Dictionary<int, float> distance = new Dictionary<int, float>();
        for (int i = 0; i < _bots.Count; i++)
        {
            BotController bot = _bots[i];

            float distanceToBot = Vector3.Distance(position, bot.transform.position);
            distance.Add(i, distanceToBot);

        }
        foreach (var item in distance.OrderBy(v => v.Value))
        {
            if (count < countBotsGets)
            {
                target.Add(_bots[item.Key]);
                count++;
            }
            // items are in sorted order


        }

        return target;
    }

    public RootTile GetRoot()
    {
        return Instantiate(_prefabRoot);
    }
}
