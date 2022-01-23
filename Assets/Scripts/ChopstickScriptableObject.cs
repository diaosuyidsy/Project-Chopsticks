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
    public float PostAttackDuration = 1f;
    public float MaxStamina = 1f;
    public float DefendStaminaCostPersec = 0.1f;
    public float PostDefendDuration = 1f;
    public HitInformation IdleHitInformation;
    public HitInformation AttackBaseHitInformation;
    public HitInformation PostDefendHitInformation;
    public float PickAnticipationMoveSpeed = 3f;
    public float PickAnticipationDuration = 2f;
    public float PickCancelDuration = 0.5f;
    public HitInformation PickAnticipationHitInformation;
    public float PickingDuration = 0.1f;
    public float PickRecoveryDuration = 0.5f;
}
