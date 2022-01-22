using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotBoiling : MonoBehaviour
{
    public Transform HotPotTransform;
}

public class HotPotSpin : MonoBehaviour
{
    public Transform HotPotTransform;
}

public class HotPotHotAir : MonoBehaviour
{
    public Transform HotPotTransform;
}

public class ChopsticksAttack : MonoBehaviour
{
    public Transform ChopsticksTransform;
}

public class ChopsticksDefence : MonoBehaviour
{
    public Transform ChopsticksTransform;
}

public class ChopsticksGetFood : MonoBehaviour
{
    public Transform ChopsticksTransform;
    public GameObject Food;
    public int Score;
}

public class ChopsticksGetNoting : MonoBehaviour
{
    public Transform ChopsticksTransform;
}

public class ChopsticksNotGetFood : MonoBehaviour
{
    public Transform ChopsticksTransform;
}

public class ChopsticksBounceBack : MonoBehaviour
{
    public Transform ChopsticksTransform;
}

public class Win : MonoBehaviour
{
    public int winner;
}

public class Lose : MonoBehaviour
{
    public int loser;
}