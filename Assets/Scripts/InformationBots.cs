using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InformationBots{
    public BotController target;
    public float distance;

    public InformationBots(BotController target, float distance)
    {
        this.target = target;
        this.distance = distance;
    }
}
