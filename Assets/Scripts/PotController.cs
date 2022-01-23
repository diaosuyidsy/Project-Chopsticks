using System;
using System.Collections.Generic;
using UnityEngine;

public class PotController : SingletonMono<PotController>
{
    public Rigidbody2D potRB;

    public float velocityChangePerRotate = 2f;
    public float rotateCooldown = 1f;
    
    public List<FoodMovementControl> foods;

    public FoodSpawner goodSpawner;
    public FoodSpawner badSpawner;
    
    public float currentAngularVelocity = 0f;
    public float deceleratePerSecond = 5f;
    
    protected override void Awake()
    {
        base.Awake();
        
        if (!potRB) potRB = GetComponent<Rigidbody2D>();
        foods = new List<FoodMovementControl>();

        // currentAngularVelocity = 10f;
    }

    private void FixedUpdate()
    {
        potRB.MoveRotation(transform.eulerAngles.z + currentAngularVelocity);

        if (currentAngularVelocity != 0f)
        {
            var deltaVel = (deceleratePerSecond * Time.fixedDeltaTime) * (currentAngularVelocity > 0f ? -1f : 1f);

            if ((currentAngularVelocity + deltaVel) * currentAngularVelocity < 0f)
            {
                currentAngularVelocity = 0f;
            }
            else
            {
                currentAngularVelocity += deltaVel;
            }
        }
        
    }

    public void ChangeAngularVelocity(bool cw, float vel)
    {
        currentAngularVelocity += (cw ? vel : -vel);
    }
    
    public void RotateUsingDefaultTorque(bool cw)
    {
        ChangeAngularVelocity(cw, velocityChangePerRotate);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!foods.Contains(other.GetComponent<FoodMovementControl>()))
        {
            foods.Add(other.GetComponent<FoodMovementControl>());
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
       /* if (other.name.Contains("food"))
        {
            Vector2 dir = Vector2.Perpendicular((Vector2) (other.transform.position - transform.position).normalized);
            other.GetComponent<FoodMovementControl>().Move(dir * angularVelocity);
        }
        #1#
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("food"))
        {
         //   other.gameObject.GetComponent<FoodMovementControl>().Move(((Vector2)other.gameObject.transform.position -other.GetContact(0).point).normalized *rb2d.angularVelocity );
          //  other.gameObject.GetComponent<Rigidbody2D>().velocity *= 0.1f;
        }
    }*/

    public void RotateFoods()
    {
        foreach (var food in foods)
        {
            Vector2 dir = Vector2.Perpendicular((Vector2) (food.transform.position - transform.position).normalized);
            dir = (Vector2)(transform.position - food.transform.position).normalized * 0.5f + dir * 0.5f; 
            food.Move(dir * potRB.angularVelocity);
        }
    }
}
