using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chopsticks", menuName = "ScriptableObject/BaseData", order = 1)]
public class ChopstickScriptableObject : ScriptableObject
{
    public float ChopsticksNormalHorizontalSpeed = 1f;
    public float ChopsticksNormalVerticalSpeed = 1f;
    public float AttackAnticipationDuration = 0.2f;
    public float AttackDuration = 1f;
    public float AttackStaminaConsumption = 0.1f;
    public float PostAttackDuration = 1f;
    public float InitialDefendStaminaCost = 0.2f;
    public float DefendStaminaCostPersec = 0.5f;
    public float PostDefendDuration = 1f;
    public HitInformation IdleHitInformation;
    public HitInformation AttackBaseHitInformation;
    public HitInformation PostDefendHitInformation;
    public float PickAnticipationMoveSpeed = 3f;
    public float PickAnticipationDuration = 2f;
    public float PickCancelDuration = 0.5f;
    public float PickStaminaCost = 0.1f;
    public HitInformation PickAnticipationHitInformation;
    public float PickingDuration = 0.1f;
    public float PickRecoveryDuration = 0.5f;
    public HitInformation PerfectReflectHitInformation;
    public HitInformation ReflectHitInformation;
    public float PerfectDefendDuration = 0.1f;
    public float RotateStaminaCost = 0.2f;
    public float PerfectBlockRestoreStamina = -0.2f;
    public float BlockRestoreStamina = 12f;
}
