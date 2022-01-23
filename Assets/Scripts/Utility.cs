using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
}

public interface IHittable
{
    void OnImpact(IHittable Enemy, bool isBlock = false, bool isReflected = false, bool isPerfectReflected = false);
    Vector2 GetHiterDirection();
    GameObject GetGameObject();
}

[System.Serializable]
public class HitInformation
{
    public float HitForce;
    public float HitDuration;
    public float StaminaCost;
    public Vector2 HiterDirection;

    public HitInformation(HitInformation copy)
    {
        HitForce = copy.HitForce;
        HitDuration = copy.HitDuration;
        StaminaCost = copy.StaminaCost;
        HiterDirection = copy.HiterDirection;
    }

    public HitInformation()
    {
        
    }
}
