using System;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private DeathZoneManager _manager;

    private void Awake()
    {
        _manager = GetComponentInParent<DeathZoneManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _manager.OnPlayerInZone();
        }
    }
}
