using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
}

public interface IHittable
{
    void OnImpact();
}

public class HitInformation
{
    public Vector2 HitForce;
    public float HitDuration;
}
