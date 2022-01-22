using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float torque = 5f;

    private float angularVelocity;

    public  List<FoodMovementControl> foods;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        foods = new List<FoodMovementControl>();
    }
    
    
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
          //  RotateForCertainAngle(true, 45);
          RotateForCertainAngleUsingForce(true, torque);
        //  RotateFoods();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            //RotateForCertainAngle(false, 45);
            RotateForCertainAngleUsingForce(false, torque);
        //    RotateFoods();
        }

      //  angularVelocity = rb2d.angularVelocity;

    }
    

    public void RotateForCertainAngle(bool cw, float angle)
    {
        transform.RotateAround(transform.position, Vector3.forward, cw? angle: -angle);
    }
    
    public void RotateForCertainAngleUsingForce(bool cw, float torque)
    {
        rb2d.AddTorque(cw ? torque : -torque, ForceMode2D.Impulse);
    }
    
    
    // Add an impulse which produces a change in angular velocity (specified in degrees).
    public void AddTorqueImpulse(float angularChangeInDegrees)
        {
            rb2d = GetComponent<Rigidbody2D>();
            var impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb2d.inertia;

            rb2d.AddTorque(impulse, ForceMode2D.Impulse);
        }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!foods.Contains(other.GetComponent<FoodMovementControl>()))
        {
            foods.Add(other.GetComponent<FoodMovementControl>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
       /* if (other.name.Contains("food"))
        {
            Vector2 dir = Vector2.Perpendicular((Vector2) (other.transform.position - transform.position).normalized);
            other.GetComponent<FoodMovementControl>().Move(dir * angularVelocity);
        }
        */
    }
    
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("food"))
        {
         //   other.gameObject.GetComponent<FoodMovementControl>().Move(((Vector2)other.gameObject.transform.position -other.GetContact(0).point).normalized *rb2d.angularVelocity );
          //  other.gameObject.GetComponent<Rigidbody2D>().velocity *= 0.1f;
        }
    }

    public void RotateFoods()
    {
        foreach (var food in foods)
        {
            Vector2 dir = Vector2.Perpendicular((Vector2) (food.transform.position - transform.position).normalized);
            dir = (Vector2)(transform.position - food.transform.position).normalized * 0.5f + dir * 0.5f; 
            food.Move(dir * rb2d.angularVelocity);
        }
    }
}
