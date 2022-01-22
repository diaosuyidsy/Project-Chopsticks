using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void RotateForCertainAngle(bool cw, float angle)
    {
        transform.RotateAround(transform.position, Vector3.forward, cw? angle: -angle);
    }
}
