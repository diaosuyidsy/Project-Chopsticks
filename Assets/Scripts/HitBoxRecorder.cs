using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxRecorder : MonoBehaviour
{
    public ChopSticksController TargetController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<IHittable>() != null && other.GetComponentInParent<IHittable>() != TargetController.GetComponent<IHittable>())
        {
            TargetController.InRangeHittables.Add(other.GetComponentInParent<IHittable>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<IHittable>() != null && other.GetComponentInParent<IHittable>() != TargetController.GetComponent<IHittable>())
        {
            TargetController.InRangeHittables.Remove(other.GetComponentInParent<IHittable>());
        }
    }
}
