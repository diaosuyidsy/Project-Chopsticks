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
    public Transform DirectionTransform;
    public ActionBarController ActionBarController;
    public Transform HandTransform;
    public Transform ClashPoint;
    
    private FSM<ChopSticksController> m_ChopstickFSM;
    private Rigidbody2D m_Rigidbody;
    public Player player;
    private HitInformation m_HitInfo = new HitInformation();
    private float m_PickMovedDuration;

    private void Awake()
    {
        m_ChopstickFSM = new FSM<ChopSticksController>(this);
        m_ChopstickFSM.TransitionTo<IdleState>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        player = ReInput.players.GetPlayer(PlayerNumber);
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
            get { return Context.player.GetAxis("LeftStickHorizontal"); }
        }
        protected float m_VAxis
        {
            get { return Context.player.GetAxis("LeftStickVertical"); }
        }

        protected bool m_Attack
        {
            get { return Context.player.GetButton("Attack"); }
        }
        
        protected bool m_Defend
        {
            get { return Context.player.GetButton("Defend"); }
        }
        
        protected bool m_Pick
        {
            get { return Context.player.GetButton("Pick"); }
        }

        public virtual void OnHit(IHittable Enemy = null, bool isBlock = false, bool isReflected = false, bool isPerfectReflected = false)
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
            Context.Animator.SetBool("Attack", false);
            Context.Animator.SetBool("Idle", false);
            Context.Animator.SetBool("Pick", false);
            Context.Animator.SetBool("Defend", false);
            Context.Animator.SetBool("PostDefend", false);

            Context.m_Rigidbody.AddForce(Context.m_HitInfo.HitForce * Context.m_HitInfo.HiterDirection, ForceMode2D.Impulse);
            Context.ActionBarController.ConsumeActionBarOneTime(Context.m_HitInfo.StaminaCost);
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
            if (m_Attack && Context.ActionBarController.ConsumeActionBarOneTime(Context.ChopstickData.AttackStaminaConsumption))
            {
                TransitionTo<AttackAnticipationState>();
                return;
            }

            if (m_Defend && Context.ActionBarController.ConsumeActionBarOneTime(Context.ChopstickData.InitialDefendStaminaCost))
            {
                TransitionTo<DefendState>();
                return;
            }

            if (m_Pick && Context.ActionBarController.ConsumeActionBarOneTime(Context.ChopstickData.PickStaminaCost))
            {
                TransitionTo<PickAnticipationState>();
                return;
            }
        }

        public override void OnHit(IHittable enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(enemy, isBlock, isReflected);
            EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnIdle, Context.HandTransform, enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
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

        public override void OnHit(IHittable Enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(Enemy);
            EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnIdle, Context.HandTransform, Enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
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
            EventManager.Instance.TriggerEvent(new ChopsticksStartAttacking(Context.HandTransform));
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
                Context.InRangeHittables[0].OnImpact(Context);
                m_AttackedOnce = true;
            }
        }

        public override void OnHit(IHittable enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            if(!isBlock)
                enemy.OnImpact(Context, true);

            if (isBlock && !isReflected && !isPerfectReflected)
            {
                Context.m_HitInfo = new HitInformation(Context.ChopstickData.AttackBaseHitInformation);
                EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnAttack, Context.HandTransform, Context.ClashPoint.position));
            }
            
            if (isBlock && isPerfectReflected)
            {
                Context.m_HitInfo = new HitInformation(Context.ChopstickData.PerfectReflectHitInformation);
            }
            
            if(isBlock && !isPerfectReflected && isReflected)
            {
                Context.m_HitInfo = new HitInformation(Context.ChopstickData.ReflectHitInformation);
            }
            
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
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.Instance.TriggerEvent(new ChopsticksDefence(Context.HandTransform));
            Context.Animator.SetBool("Defend", true);
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            Context.m_Rigidbody.velocity = Vector2.zero;
            if (!m_Defend)
            {
                TransitionTo<PostDefendState>();
                return;
            }

            if (!Context.ActionBarController.ConsumeActionBarContinuously(Context.ChopstickData.DefendStaminaCostPersec))
            {
                TransitionTo<PostDefendState>();
                return;
            }

            m_Timer += Time.deltaTime;
        }

        public override void OnHit(IHittable Enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(Enemy);
            if (m_Timer <= Context.ChopstickData.PerfectDefendDuration)
            {
                EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnPerfectDefend, Enemy.GetGameObject().GetComponent<ChopSticksController>().HandTransform, Enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
                Enemy?.OnImpact(Context, true, true, true);
                Context.ActionBarController.RecoverActionBar(Context.ChopstickData.PerfectBlockRestoreStamina);
            }
            else
            {
                EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnDefend, Enemy.GetGameObject().GetComponent<ChopSticksController>().HandTransform, Enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
                Enemy?.OnImpact(Context, true, true, false);
                Context.ActionBarController.RecoverActionBar(Context.ChopstickData.BlockRestoreStamina);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Context.Animator.SetBool("Defend", false);
        }
    }

    private class PostDefendState : ChopstickStates
    {
        private float m_Timer;

        public override void OnEnter()
        {
            base.OnEnter();
            EventManager.Instance.TriggerEvent(new ChopsticksDefenceCancel(Context.HandTransform));
            Context.Animator.SetBool("PostDefend", true);
            m_Timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            if (m_Defend && Context.ActionBarController.ConsumeActionBarOneTime(Context.ChopstickData.InitialDefendStaminaCost))
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
        
        public override void OnHit(IHittable enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(enemy);
            EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnIdle, Context.HandTransform, enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.PostDefendHitInformation);
            Context.m_HitInfo.HiterDirection = enemy.GetHiterDirection();
            TransitionTo<ReflectedState>();
            return;
        }
        
        public override void OnExit()
        {
            base.OnExit();
            Context.Animator.SetBool("PostDefend", false);
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
            Context.Animator.SetFloat("PickSpeed", 1f);
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
            var newPosition = Context.m_Rigidbody.position + new Vector2(m_HAxis * Context.ChopstickData.PickAnticipationMoveSpeed, m_VAxis * Context.ChopstickData.PickAnticipationMoveSpeed) * Time.fixedDeltaTime;
            Context.m_Rigidbody.MovePosition(newPosition);
        }

        public override void OnHit(IHittable Enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(Enemy, isBlock);
            EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnIdle, Context.HandTransform, Enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.PickAnticipationHitInformation);
            Context.m_HitInfo.HiterDirection = Enemy.GetHiterDirection();
            Context.Animator.SetBool("Pick", false);
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
            m_TargetPosition = Context.transform.position - Context.DirectionTransform.right * Context.m_PickMovedDuration;
            Context.Animator.SetFloat("PickSpeed", -1f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var nextpos = Vector3.Lerp(m_InitialPosition, m_TargetPosition,
                m_Timer / Context.ChopstickData.PickCancelDuration);
            Context.m_Rigidbody.MovePosition(nextpos);
        }

        public override void Update()
        {
            base.Update();
            if (m_Timer < Context.ChopstickData.PickCancelDuration)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= Context.ChopstickData.PickCancelDuration)
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
                    PickupSuccess();
                    TransitionTo<PickRecoveryState>();
                    return;
                }
            }
        }

        public override void OnHit(IHittable Enemy, bool isBlock, bool isReflected, bool isPerfectReflected)
        {
            base.OnHit(Enemy, isBlock);
            EventManager.Instance.TriggerEvent(new ChopsticksClash(ChopsticksClash.AttackType.AttackOnIdle, Context.HandTransform, Enemy.GetGameObject().GetComponent<ChopSticksController>().ClashPoint.position));
            Context.m_HitInfo = new HitInformation(Context.ChopstickData.PickAnticipationHitInformation);
            Context.m_HitInfo.HiterDirection = Enemy.GetHiterDirection();
            Context.Animator.SetBool("Pick", false);
            TransitionTo<ReflectedState>();
        }

        private void PickupSuccess()
        {
            if (Context.FoodRecorder.FoodInRange.Count != 0)
            {
                EventManager.Instance.TriggerEvent(new ChopsticksGetFood(Context.HandTransform,
                    Context.FoodRecorder.FoodInRange[0].gameObject,
                    Context.FoodRecorder.FoodInRange[0].GetComponent<FoodBase>().score));
                Context.FoodRecorder.OnConsumeFood();
            }
            else
            {
                EventManager.Instance.TriggerEvent(new ChopsticksNotGetFood());
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

    public void OnImpact(IHittable Enemy, bool isBlock = false, bool isReflected = false, bool isPerfectReflected = false)
    {
        (m_ChopstickFSM.CurrentState as ChopstickStates).OnHit(Enemy, isBlock, isReflected, isPerfectReflected);
    }

    public Vector2 GetHiterDirection()
    {
        return -HitBox.right;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}