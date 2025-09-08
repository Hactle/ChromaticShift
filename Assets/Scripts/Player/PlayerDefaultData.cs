using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDefaultData", menuName = "Player/PlayerDefaultData")]
public class PlayerDefaultData : ScriptableObject
{
    [Header("Movement Defaults")]
    [Range(0, 1)] public float IceAccelerationMultiplier;
    [Range(0, 1)] public float IceDecelerationMultiplier;
    public float MoveSpeed;
    public float Acceleration;
    public float Deceleration;
    public float ClimbSpeed;

    [Header("Jump Defaults")]
    [Range(0, 1)] public float AirControlMultiplier;
    public float JumpForce;
    public float HoldMultiplier;
    public float FallMultiplier;
    public float LowMultiplier;
    public float MaxHoldTime;
}
