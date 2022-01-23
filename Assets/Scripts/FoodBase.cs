using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBase : MonoBehaviour
{
    private SpriteRenderer sr;
    private SpringJoint2D _springJoint2D;
    public int Score = 2;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        _springJoint2D = GetComponentInChildren<SpringJoint2D>(true);
        col = GetComponentInChildren<Collider2D>(true);

    }


    public IEnumerator SpawnRoutine()
    {
        gameObject.SetActive(true);
      //  sr.sortingOrder = -1;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
        _springJoint2D.enabled = true;
        sr.sortingOrder = 2;

    }
}
