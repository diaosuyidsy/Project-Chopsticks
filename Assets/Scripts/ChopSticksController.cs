using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChopSticksController : MonoBehaviour, IHittable
{
    public int PlayerNumber;
    public ChopstickScriptableObject ChopstickData;
    public HashSet<IHittable> InRangeHittables = new HashSet<IHittable>();
    
    private FSM<ChopSticksController> m_ChopstickFSM;
    private Rigidbody2D m_Rigidbody;
    private Player m_Player;
    private HitInformation m_HitInfo = new HitInformation();

    private void Awake()
    {
        m_ChopstickFSM = new FSM<ChopSticksController>(this);
        m_ChopstickFSM.TransitionTo<IdleState>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Player = ReInput.players.GetPlayer(PlayerNumber);
    }

    private void Update()
    {
        m_ChopstickFSM.Update();
    }

    private void FixedUpdate()
    {
        m_ChopstickFSM.FixedUpdate();
    }

    private void LateUpdate()
    {
        m_ChopstickFSM.LateUpdate();
    }

    private abstract class ChopstickStates : FSM<ChopSticksController>.State
    {
        protected float m_HAxis
        {
            get { return Context.m_Player.GetAxis("LeftStickHorizontal"); }
        }
        protected float m_VAxis
        {
            get { return Context.m_Player.GetAxis("LeftStickVertical"); }
        }

        protected bool m_Attack
        {
            get { return Context.m_Player.GetButton("Attack"); }
        }
        
        protected bool m_Defend
        {
            get { return Context.m_Player.GetButton("Defend"); }
        }
        
        protected bool m_Pick
        {
            get { return Context.m_Player.GetButton("Pick"); }
        }

        public virtual void OnHit()
        {
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            print(GetType().Name);
        }
    }

    /// <summary>
    /// 被弹反的状态
    /// </summary>
    private class ReflectedState : ChopstickStates
    {
        private float m_Timer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            Context.m_Rigidbody.AddForce(Context.m_HitInfo.HitForce * -Context.transform.up, ForceMode2D.Impulse);
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Timer < Context.m_HitInfo.HitDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.m_HitInfo.HitDuration)
                {
                    TransitionTo<IdleState>();
                    return;
                }
            }
        }
    }

    private class IdleState : ChopstickStates
    {
        public override void Update()
        {
            base.Update();
            if (m_Attack)
            {
                TransitionTo<AttackAnticipationState>();
                return;
            }

            if (m_Defend)
            {
                TransitionTo<DefendState>();
                return;
            }
        }

        public override void OnHit()
        {
            base.OnHit();
            Context.m_HitInfo = Context.ChopstickData.IdleHitInformation;
            TransitionTo<ReflectedState>();
            return;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var newPosition = Context.m_Rigidbody.position + new Vector2(m_HAxis * Context.ChopstickData.ChopsticksNormalHorizontalSpeed, m_VAxis * Context.ChopstickData.ChopsticksNormalVerticalSpeed) * Time.fixedDeltaTime;
            Context.m_Rigidbody.MovePosition(newPosition);
        }
    }

    private class AttackBaseState : ChopstickStates
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var newPosition = Context.m_Rigidbody.position + new Vector2(m_HAxis * Context.ChopstickData.ChopsticksNormalHorizontalSpeed, m_VAxis * Context.ChopstickData.ChopsticksNormalVerticalSpeed) * Time.fixedDeltaTime;
            Context.m_Rigidbody.MovePosition(newPosition);
        }
    }

    private class AttackAnticipationState : AttackBaseState
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Timer < Context.ChopstickData.AttackAnticipationDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.AttackAnticipationDuration)
                {
                    TransitionTo<AttackState>();
                    return;
                }
            }
        }
    }

    private class AttackState : AttackBaseState
    {
        private float m_Timer;
        private bool m_AttackedOnce = false;

        public override void OnEnter()
        {
            base.OnEnter();
            m_AttackedOnce = false;
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Timer < Context.ChopstickData.AttackDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.AttackDuration)
                {
                    TransitionTo<PostAttackState>();
                    return;
                }
            }

            if (Context.InRangeHittables.Count != 0 && !m_AttackedOnce)
            {
                foreach (var hittable in Context.InRangeHittables)
                {
                    hittable.OnImpact();
                }

                m_AttackedOnce = true;
            }
        }

        public override void OnHit()
        {
            base.OnHit();
            TransitionTo<ReflectedState>();
            return;
        }
    }
    
    private class PostAttackState : AttackBaseState
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Timer < Context.ChopstickData.PostAttackDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PostAttackDuration)
                {
                    TransitionTo<IdleState>();
                    return;
                }
            }
        }
    }

    private class DefendState : ChopstickStates
    {
        public override void Update()
        {
            base.Update();
            if (!m_Defend)
            {
                TransitionTo<PostDefendState>();
                return;
            }
        }
    }

    private class PostDefendState : ChopstickStates
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Defend)
            {
                TransitionTo<DefendState>();
                return;
            }

            if (m_Timer < Context.ChopstickData.PostDefendDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PostDefendDuration)
                {
                    TransitionTo<IdleState>();
                    return;
                }
            }
        }
    }

    public void OnImpact()
    {
        (m_ChopstickFSM.CurrentState as ChopstickStates).OnHit();
    }
}