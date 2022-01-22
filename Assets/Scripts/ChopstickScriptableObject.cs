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
}
