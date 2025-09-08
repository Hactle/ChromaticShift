using UnityEngine;
using Input;
using System;

public enum StateColor
{
    red,
    green,
    blue,
    grey
}

public class ColorChanger : MonoBehaviour
{
    public static event Action<StateColor> OnColorChanged;

    private PlayerInputHandler _playerInput;

    private StateColor _currentColor;
    public StateColor CurrentColor
    {
        get => _currentColor;
        set
        {
            _currentColor = value;
            OnColorChanged?.Invoke(value);
        }
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
    }

    private void ChangeStateColor(StateColor newColor)
    {
        CurrentColor = newColor;
        Debug.Log("Current color =" + CurrentColor);
    }

    private void OnEnable()
    {
        _playerInput.OnColorChange += ChangeStateColor;
    }

    private void OnDisable()
    {
        _playerInput.OnColorChange -= ChangeStateColor;
    }
}

