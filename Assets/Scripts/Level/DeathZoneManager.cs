using System;
using UnityEngine;

public class DeathZoneManager : MonoBehaviour
{
    public event Action OnPlayerDeath;

    public void OnPlayerInZone()
    {
        OnPlayerDeath?.Invoke();
    }
}
