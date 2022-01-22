using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodMovementControl : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float forceCoefficient = 0.01f; 
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 currentForce;
    public void Move(Vector2 force)
    {
        currentForce = force;
        rb2d.AddForce(force * forceCoefficient, ForceMode2D.Impulse);
       
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawLine(transform.position, (Vector2)transform.position+currentForce * forceCoefficient );
    }
}
