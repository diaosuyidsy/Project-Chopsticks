using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    private RectTransform _rectTransform;

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
            localScale = _rectTransform.localScale;
            localScale.x -= Time.fixedDeltaTime*0.1f;
            value -= Time.fixedDeltaTime*0.1f;
            _rectTransform.localScale = localScale;
            yield return null;
        }
    }

    public bool ConsumeActionBarContinuously()
    {
       if (_rectTransform.localScale.x >0)
        {
            localScale = _rectTransform.localScale;
            localScale.x -= Time.fixedDeltaTime*0.1f;
            localScale.x = Mathf.Max(0, localScale.x);
            _rectTransform.localScale = localScale;
            return true;
        }
       else
       {
           return false;
       }
    }

    public IEnumerator RecoverActionBarRoutine()
    {
        while (_rectTransform.localScale.x <1)
        {
            localScale = _rectTransform.localScale;
            localScale = _rectTransform.localScale;
            localScale.x += Time.fixedDeltaTime*0.1f;
            localScale.x = Mathf.Min(1, localScale.x);
            _rectTransform.localScale = localScale;
            yield return null;
        }
    }

    public bool RecoverActionBar()
    {
        StartCoroutine(RecoverActionBarRoutine());
        return false;
    }

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
           ConsumeActionBarContinuously();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            RecoverActionBar();
        }
    }



}
