using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVFXCtrl : MonoBehaviour
{
    public float lowTime = 1;
    public float highTime = 5;
    public float lowScale = 0.2f;
    public float highScale = 0.8f;
    public float lowRotation = 0;
    public float highRotation = 360;
    private float _timeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _timeCount = Random.Range(lowTime,highTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnime()
    {
        var scal = Random.Range(lowScale,highScale);
        //var rota = Random.Range(lowRotation,highRotation);
        this.transform.localScale = new Vector3(scal, scal, scal);
        //this.transform.localEulerAngles = new Vector3(0, 0, rota);
        this.transform.GetComponent<Animator>().SetInteger("state",1);
    }
}
