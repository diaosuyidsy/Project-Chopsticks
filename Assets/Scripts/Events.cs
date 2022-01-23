using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotBoiling : GameEvent
{
    public Transform HotPotTransform;
}

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
    public Transform ChopsticksTransform;
}

public class ChopsticksDefence : GameEvent
{
    public Transform ChopsticksTransform;
}

public class ChopsticksGetFood : GameEvent
{
    public Transform ChopsticksTransform;
    public GameObject Food;
    public int Score;
}

public class ChopsticksGetNoting : GameEvent
{
    public Transform ChopsticksTransform;
}

public class ChopsticksNotGetFood : GameEvent
{
    public Transform ChopsticksTransform;
}

public class ChopsticksBounceBack : GameEvent
{
    public Transform ChopsticksTransform;
}

public class Win : GameEvent
{
    public int winner;
}

public class Lose : GameEvent
{
    public int loser;
}