using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotSpin : GameEvent
{
    public Transform HotPotTransform;
}

public class HotPotHotAir : GameEvent
{
    public Transform HotPotTransform;
}

public class ChopsticksAttack : GameEvent
{
    public enum AttackType
    {
        AttackOnIdle,
        AttackOnAttack,
        AttackOnDefend,
        AttackOnPerfectDefend
    }

    public AttackType AttType;
    public Transform HandTransform;

    public ChopsticksAttack(AttackType attackType, Transform handTransform)
    {
        AttType = attackType;
        HandTransform = handTransform;
    }
}

/// <summary>
/// 防御开始
/// </summary>
public class ChopsticksDefence : GameEvent
{
    public Transform HandTransform;

    public ChopsticksDefence(Transform handTransform)
    {
        HandTransform = handTransform;
    }
}

/// <summary>
/// 防御结束
/// </summary>
public class ChopsticksDefenceCancel : GameEvent
{
    public Transform HandTransform;

    public ChopsticksDefenceCancel(Transform handTransform)
    {
        HandTransform = handTransform;
    }
}

public class ChopsticksGetFood : GameEvent
{
    public Transform HandTransform;
    public GameObject Food;
    public int Score;

    public ChopsticksGetFood(Transform handTransform, GameObject food, int score)
    {
        HandTransform = handTransform;
        Food = food;
        Score = score;
    }
}

public class ChopsticksNotGetFood : GameEvent
{
    public Transform HandTransform;
}

public class ChopsticksBounceBack : GameEvent
{
    public Transform HandTransform;
}

public class Win : GameEvent
{
    public int winner;
}

public class Lose : GameEvent
{
    public int loser;
}