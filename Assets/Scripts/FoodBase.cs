using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBase : MonoBehaviour
{
    public bool isGoodFood;
    
    private SpriteRenderer m_Renderer;
    private SpringJoint2D m_SpringJoint;
    private Collider2D m_Collider;

    public Transform spawnTransform { get; set; }

    public int score = 2;
    
    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_SpringJoint = GetComponentInChildren<SpringJoint2D>(true);
        m_Collider = GetComponentInChildren<Collider2D>(true);

        isGoodFood = score > 0;
        // 正分被当作好食物
    }

    public IEnumerator SetFoodActive()
    {
        yield return new WaitForSeconds(0.5f);
        
        m_Renderer.sortingOrder = 2;
        
        // m_SpringJoint.connectedAnchor = transform.position;
        // m_SpringJoint.enabled = true;
        
        m_Collider.enabled = true;
    }

/*public IEnumerator SpawnRoutine()
{
    gameObject.SetActive(true);
  //  sr.sortingOrder = -1;
    yield return new WaitForSeconds(0.5f);
    col.enabled = true;
    _springJoint2D.enabled = true;
    sr.sortingOrder = 2;
}*/
}
