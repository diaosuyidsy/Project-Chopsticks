using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    private RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private Vector3 localScale;
    public IEnumerator ConsumeActionBarOneTime(float value)
    {
        while (_rectTransform.localScale.x >0 && value>=0)
        {
            localScale = _rectTransform.localScale;
            localScale.x -= Time.deltaTime;
            value -= Time.deltaTime;
            _rectTransform.localScale = localScale;
            yield return null;
        }
    }

    public void ConsumeActionBarContinuously()
    {
       if (_rectTransform.localScale.x >0)
        {
            localScale = _rectTransform.localScale;
            localScale.x -= Time.deltaTime;
            localScale.x = Mathf.Max(0, localScale.x);
            _rectTransform.localScale = localScale;
        }
    }

    public IEnumerator RecoverActionBar()
    {
        while (_rectTransform.localScale.x <1)
        {
            localScale = _rectTransform.localScale;
            localScale.x += Time.deltaTime;
            localScale.x = Mathf.Min(1, localScale.x);
            _rectTransform.localScale = localScale;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ConsumeActionBarOneTime(0.1f));
        }
        if (Input.GetKey(KeyCode.V))
        {
           // StartCoroutine(ConsumeActionBarContinuously());
           ConsumeActionBarContinuously();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(RecoverActionBar());
        }
    }
    
    
}
