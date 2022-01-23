using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    private RectTransform _rectTransform;
    //public float continuouslyDropValue = 0.01f;
    //public float dropOnceValue = 0.1f;
    //public float recoverSpeed= 0.1f;
    public float recoverPulse = 1f;
    

    private float consumeStaminaBarTime;
  

  //  private bool canConsume;

   // float currentScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private Vector3 localScale;

    public bool ConsumeActionBarOneTime(float value)
    {
        if (_rectTransform.localScale.x< value)
        {
            return false;
        }
        else
        {
            StartCoroutine(ConsumeActionBarOneTimeRoutine(value));
            return true;
        }
    }



    public IEnumerator ConsumeActionBarOneTimeRoutine(float value)
    {
        while (_rectTransform.localScale.x >0 && value>=0)
        {
          //  canRecoverStamina = false;
            consumeStaminaBarTime = Time.realtimeSinceStartup;
            localScale = _rectTransform.localScale;
            localScale.x -= Time.fixedDeltaTime*0.1f;
            value -= Time.fixedDeltaTime*0.1f;
            _rectTransform.localScale = localScale;
            yield return null;
        }

      //  canRecoverStamina = true;
    }

    public bool ConsumeActionBarContinuously(float value)
    {
       if (_rectTransform.localScale.x >0)
       {
          // canRecoverStamina = false;
            consumeStaminaBarTime = Time.realtimeSinceStartup;
            localScale = _rectTransform.localScale;
            localScale.x -= Time.fixedDeltaTime*value;
            localScale.x = Mathf.Max(0, localScale.x);
            _rectTransform.localScale = localScale;
            return true;
        }
       else
       {
          // canRecoverStamina = true;
           return false;
       }
    }

    private bool recoverActionBarRoutineStarted = false;
    public IEnumerator RecoverActionBarRoutine()
    {
        while (_rectTransform.localScale.x <1)
        {
            recoverActionBarRoutineStarted = true;
            localScale = _rectTransform.localScale;
            localScale = _rectTransform.localScale;
            localScale.x += Time.fixedDeltaTime*0.01f;
            localScale.x = Mathf.Min(1, localScale.x);
            _rectTransform.localScale = localScale;
            yield return null;
        }

        recoverActionBarRoutineStarted = false;
    }

    public bool RecoverActionBar(float value)
    {
        if(_rectTransform.localScale.x <1)
        {
            recoverActionBarRoutineStarted = true;
            localScale = _rectTransform.localScale;
            localScale = _rectTransform.localScale;
            localScale.x += Time.fixedDeltaTime*value;
            localScale.x = Mathf.Min(1, localScale.x);
            _rectTransform.localScale = localScale;
        }
        
      //  if(!recoverActionBarRoutineStarted)
       // StartCoroutine(RecoverActionBarRoutine());
        return false;
    }

    private float recoverTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConsumeActionBarOneTime(0.1f);
        }
        if (Input.GetKey(KeyCode.V))
        {
           // StartCoroutine(ConsumeActionBarContinuously());
           ConsumeActionBarContinuously(0.01f);
        }


        if ((Time.realtimeSinceStartup - consumeStaminaBarTime) > recoverPulse)
        {
            RecoverActionBar(0.01f);
        }
        
    }



}
