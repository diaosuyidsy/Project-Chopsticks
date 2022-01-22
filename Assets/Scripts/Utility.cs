using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
}

public interface IHittable
{
    void OnImpact(IHittable Enemy, bool isBlock = false);
}

[System.Serializable]
public class HitInformation
{
    public float HitForce;
    public float HitDuration;
    public float StaminaCost;
}
