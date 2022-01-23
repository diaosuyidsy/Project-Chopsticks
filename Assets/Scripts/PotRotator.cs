using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PotRotator : MonoBehaviour
{
    private Player m_Player;
    private ActionBarController m_ActionBarController;
    private ChopstickScriptableObject m_Data;

    public bool leftSpinClockWise;
    
    private float m_Timer = 0f;

    protected bool m_LeftRotate
    {
        get { return m_Player.GetButton("LeftRotate"); }
    }
        
    protected bool m_RightRotate
    {
        get { return m_Player.GetButton("RightRotate"); }
    }
    
    void Start()
    {
        var chopsticks = GetComponent<ChopSticksController>();

        m_Player = chopsticks.player;
        m_ActionBarController = chopsticks.ActionBarController;
        m_Data = chopsticks.ChopstickData;
    }
    
    void Update()
    {
        if (m_Timer >= PotController.singleton.rotateCooldown)
        {
            if (m_LeftRotate && m_ActionBarController.ConsumeActionBarOneTime(m_Data.RotateStaminaCost))
            {
                PotController.singleton.RotateUsingDefaultTorque(leftSpinClockWise);
                m_Timer = 0f;
            }
        
            if (m_RightRotate && m_ActionBarController.ConsumeActionBarOneTime(m_Data.RotateStaminaCost))
            {
                PotController.singleton.RotateUsingDefaultTorque(!leftSpinClockWise);
                m_Timer = 0f;
            }
        }

        m_Timer += Time.deltaTime;
    }
}
