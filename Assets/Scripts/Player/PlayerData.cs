using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public PlayerDefaultData DefaultData;

    [Header("Current runtime values")]

    [Header("Movement")]
    [Space(5)]
    [Range(0, 1)] public float IceAccelerationMultiplier;
    [Range(0, 1)] public float IceDecelerationMultiplier;
    public float MoveSpeed;
    public float Acceleration;
    public float Deceleration;
    public float ClimbSpeed;
    public float DashForce;
    public float DashCooldown;
    public bool CanClimb;
    public bool CanDash;

    [Header("Jump")]
    [Space(5)]
    [Range(0, 1)] public float AirControlMultiplier;
    public float JumpForce;
    public float HoldMultiplier;
    public float FallMultiplier;
    public float LowMultiplier;
    public float MaxHoldTime;

    private void ChangeParameters(StateColor actualColor)
    {
        switch (actualColor)
        {
            case StateColor.red:
                ResetFields();
                CanDash = true;
                MoveSpeed += 2;
                break;
            case StateColor.green:
                ResetFields();
                CanClimb = true;
                break;
            case StateColor.blue:
                ResetFields();
                IceAccelerationMultiplier = 1f;
                IceDecelerationMultiplier = 1f;
                JumpForce = 11f;
                break;
            case StateColor.grey:
                ResetFields(); break;               
        }
    }

    private void ResetFields()
    {
        MoveSpeed = DefaultData.MoveSpeed;
        Acceleration = DefaultData.Acceleration;
        Deceleration = DefaultData.Deceleration;
        IceAccelerationMultiplier = DefaultData.IceAccelerationMultiplier;
        IceDecelerationMultiplier = DefaultData.IceDecelerationMultiplier;    
        ClimbSpeed = DefaultData.ClimbSpeed;
        CanClimb = false;
        CanDash = false;

        AirControlMultiplier = DefaultData.AirControlMultiplier;
        JumpForce = DefaultData.JumpForce;
        HoldMultiplier = DefaultData.HoldMultiplier;
        FallMultiplier = DefaultData.FallMultiplier;
        LowMultiplier = DefaultData.LowMultiplier;
        MaxHoldTime = DefaultData.MaxHoldTime;
    }

    private void OnEnable()
    {
        if (DefaultData == null)
        {
            Debug.LogError("PlayerDefaultData not assigned in PlayerData!");
            return;
        }

        ResetFields();

        ColorChanger.OnColorChanged += ChangeParameters;
    }

    private void OnDisable()
    {
        ColorChanger.OnColorChanged -= ChangeParameters;
    }
}
