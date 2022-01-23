using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCtrl : MonoBehaviour
{
    public Transform bubble;
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
        if (_timeCount - Time.deltaTime <= 0)
        {
            PlayAnime();
            _timeCount = Random.Range(lowTime,highTime);
        }
        else
        {
            _timeCount -= Time.deltaTime;
        }
    }

    public void PlayAnime()
    {
        var scal = Random.Range(lowScale,highScale);
        var rota = Random.Range(lowRotation,highRotation);
        bubble.localScale = new Vector3(scal, scal, scal);
        bubble.localEulerAngles = new Vector3(0, 0, rota);
        bubble.GetComponent<Animator>().SetInteger("state",1);
    }
}
