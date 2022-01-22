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

[System.Serializable]
public class HitInformation
{
    public float HitForce;
    public float HitDuration;
    public float StaminaCost;
}
