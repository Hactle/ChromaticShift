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
    private SpriteRenderer _spriteRenderer;
    private PlayerInputHandler _playerInput;

    public static event Action<StateColor> OnColorChanged;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInputHandler>();
        CurrentColor = StateColor.grey;
    }

    private void SetPlayerColor(StateColor color)
    {
        switch (color)
        {
            case StateColor.red:
                _spriteRenderer.color = Color.red; break;
            case StateColor.green:
                _spriteRenderer.color = Color.green; break;
            case StateColor.blue:
                _spriteRenderer.color = Color.blue; break;
            case StateColor.grey:
                _spriteRenderer.color = Color.grey; break;
        }
    }

    private void ChangeStateColor(StateColor newColor)
    {
        CurrentColor = newColor;     
        SetPlayerColor(newColor);

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

