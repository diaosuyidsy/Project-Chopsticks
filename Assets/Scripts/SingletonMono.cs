using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_Singleton;
    public static T singleton => m_Singleton;

    protected virtual void Awake()
    {
        if (m_Singleton)
        {
            Debug.LogErrorFormat("Another singleton <{0}> on game object [{1}] already exists! " +
                                   "Not creating another one on [{2}]",
                typeof(T), m_Singleton.gameObject.name, gameObject.name);
            
            Destroy(this);
            return;
        }

        m_Singleton = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (m_Singleton == this as T)
        {
            m_Singleton = null;
            Debug.LogFormat("Destroying singleton [{0}] on game object [{1}]", typeof(T), gameObject.name);
        }
    }
}
