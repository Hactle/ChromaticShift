using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private LevelsData _data;
    [SerializeField] private List<Button> _buttons;


    private void Awake()
    {
        UpdateButtonsInteractable();
    }

    private void UpdateButtonsInteractable()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = _data.levels[i + 1].IsUnlocked;
        }
    }

    private void OnEnable()
    {
        _data.OnLevelUnlock += UpdateButtonsInteractable;
    }

    private void OnDisable()
    {
        _data.OnLevelUnlock -= UpdateButtonsInteractable;
    }
}
