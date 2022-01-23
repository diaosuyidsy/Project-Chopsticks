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
    public List<IHittable> InRangeHittables = new List<IHittable>();
    public FoodRecorder FoodRecorder;
    public Animator Animator;
    public Transform HitBox;
    
    private FSM<ChopSticksController> m_ChopstickFSM;
    private Rigidbody2D m_Rigidbody;
    private Player m_Player;
    private HitInformation m_HitInfo = new HitInformation();
    private float m_PickMovedDuration;

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

        public virtual void OnHit(IHittable Enemy = null, bool isBlock = false)
        {
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            print(GetType().Name + Context.gameObject.name);
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
            Context.m_Rigidbody.AddForce(Context.m_HitInfo.HitForce * Context.m_HitInfo.HiterDirection, ForceMode2D.Impulse);
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

        public override void OnExit()
        {
            base.OnExit();
            Context.m_Rigidbody.velocity = Vector2.zero;
        }
    }

    private class IdleState : ChopstickStates
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.Animator.SetBool("Idle", true);
        }

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

            if (m_Pick)
            {
                TransitionTo<PickAnticipationState>();
                return;
            }
        }

        public override void OnHit(IHittable enemy, bool isBlock = false)
        {
            base.OnHit(enemy);
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.IdleHitInformation);
            Context.m_HitInfo.HiterDirection = enemy.GetHiterDirection();
            TransitionTo<ReflectedState>();
            return;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var newPosition = Context.m_Rigidbody.position + new Vector2(m_HAxis * Context.ChopstickData.ChopsticksNormalHorizontalSpeed, m_VAxis * Context.ChopstickData.ChopsticksNormalVerticalSpeed) * Time.fixedDeltaTime;
            Context.m_Rigidbody.MovePosition(newPosition);
        }

        public override void OnExit()
        {
            base.OnExit();
            Context.Animator.SetBool("Idle", false);
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

        public override void OnHit(IHittable Enemy = null, bool isBlock = false)
        {
            base.OnHit(Enemy);
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.AttackBaseHitInformation);
            Context.m_HitInfo.HiterDirection = Enemy.GetHiterDirection();
            Context.Animator.SetBool("Attack", false);
            TransitionTo<ReflectedState>();
            return;
        }
    }

    private class AttackAnticipationState : AttackBaseState
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
            Context.Animator.SetBool("Attack", true);
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
                    hittable.OnImpact(Context);
                }

                m_AttackedOnce = true;
            }
        }

        public override void OnHit(IHittable enemy, bool isBlock = false)
        {
            if(!isBlock)
                enemy.OnImpact(Context, true);
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.AttackBaseHitInformation);
            if(!isBlock)
                Context.m_HitInfo.HiterDirection = enemy.GetHiterDirection();
            else
                Context.m_HitInfo.HiterDirection = -Context.GetHiterDirection();
            Context.Animator.SetBool("Attack", false);
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

        public override void OnExit()
        {
            base.OnExit();
            Context.Animator.SetBool("Attack", false);
        }
    }

    private class DefendState : ChopstickStates
    {
        public override void Update()
        {
            base.Update();
            Context.m_Rigidbody.velocity = Vector2.zero;
            if (!m_Defend)
            {
                TransitionTo<PostDefendState>();
                return;
            }
        }

        public override void OnHit(IHittable Enemy, bool isBlock = false)
        {
            base.OnHit(Enemy);
            Enemy?.OnImpact(Context, true);
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
        
        public override void OnHit(IHittable enemy, bool isBlock = false)
        {
            base.OnHit(enemy);
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.PostDefendHitInformation);
            Context.m_HitInfo.HiterDirection = enemy.GetHiterDirection();
            TransitionTo<ReflectedState>();
            return;
        }
    }

    private class PickAnticipationState : ChopstickStates
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
            Context.Animator.SetBool("Pick", true);
        }

        public override void Update()
        {
            base.Update();
            if (!m_Pick)
            {
                Context.m_PickMovedDuration = m_Timer;
                TransitionTo<PickCancelState>();
                return;
            }

            if (m_Timer < Context.ChopstickData.PickAnticipationDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PickAnticipationDuration)
                {
                    TransitionTo<PickingState>();
                    return;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var nextPos = Context.transform.up * (Context.ChopstickData.PickAnticipationMoveSpeed * Time.fixedDeltaTime) + Context.transform.position;
            Context.m_Rigidbody.MovePosition(nextPos);
        }

        public override void OnHit(IHittable Enemy = null, bool isBlock = false)
        {
            base.OnHit(Enemy, isBlock);
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.PickAnticipationHitInformation);
            Context.m_HitInfo.HiterDirection = Enemy.GetHiterDirection();
            TransitionTo<ReflectedState>();
        }
    }

    private class PickCancelState : ChopstickStates
    {
        private float m_Timer;
        private Vector3 m_TargetPosition;
        private Vector3 m_InitialPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            m_Timer = 0f;
            m_InitialPosition = new Vector3(Context.transform.position.x, Context.transform.position.y,
                Context.transform.position.z);
            m_TargetPosition = Context.transform.position - Context.transform.up * Context.m_PickMovedDuration;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var nextpos = Vector3.Lerp(m_InitialPosition, m_TargetPosition,
                m_Timer / Context.ChopstickData.PickCancelDuration);
            Context.m_Rigidbody.MovePosition(nextpos);
            if (m_Timer < Context.ChopstickData.PickCancelDuration)
            {
                m_Timer += Time.fixedDeltaTime;
                if (m_Timer >= Context.ChopstickData.PickCancelDuration)
                {
                    TransitionTo<IdleState>();
                    return;
                }
            }
        }
    }

    private class PickingState : ChopstickStates
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
            if (m_Timer < Context.ChopstickData.PickingDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PickingDuration)
                {
                    TransitionTo<PickRecoveryState>();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PickupSuccess();
        }

        private void PickupSuccess()
        {
            if (Context.FoodRecorder.FoodInRange.Count != 0)
            {
                print("Pickedupfood!");
            }
            else
            {
                print("Failed");
            }
        }
    }

    private class PickRecoveryState : ChopstickStates
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
            if (m_Timer < Context.ChopstickData.PickRecoveryDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PickRecoveryDuration)
                {
                    TransitionTo<IdleState>();
                    return;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Context.Animator.SetBool("Pick", false);
        }
    }

    public void OnImpact(IHittable Enemy, bool isBlock = false)
    {
        (m_ChopstickFSM.CurrentState as ChopstickStates).OnHit(Enemy, isBlock);
    }

    public Vector2 GetHiterDirection()
    {
        return -HitBox.right;
    }
}