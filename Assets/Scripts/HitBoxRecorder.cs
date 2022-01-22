using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxRecorder : MonoBehaviour
{
    public ChopSticksController TargetController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IHittable>() != null)
        {
            TargetController.InRangeHittables.Add(other.GetComponent<IHittable>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IHittable>() != null && other.GetComponent<IHittable>() != TargetController.GetComponent<IHittable>())
        {
            TargetController.InRangeHittables.Remove(other.GetComponent<IHittable>());
        }
    }
}
