using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour, IColorDepended
{
    [SerializeField] private StateColor _selfColor;

    private Collider2D _selfCollider;

    private void Awake()
    {
        _selfCollider = GetComponent<Collider2D>();
        if(_selfColor == StateColor.grey)
        {
            Destroy(gameObject);
            Debug.LogWarning("Door color can't be grey. Doors with color type automaticly destroyed");
        }
    }

    private void CheckCollisionByColor(StateColor playerColor)
    {
        bool shouldIgnore = (playerColor == _selfColor);
        _selfCollider.enabled = !shouldIgnore;
    }

    public void OnColorChange(StateColor newColor)
    {
        CheckCollisionByColor(newColor);
    }

    private void OnEnable()
    {
        ColorChanger.OnColorChanged += OnColorChange;
    }

    private void OnDisable()
    {
        ColorChanger.OnColorChanged -= OnColorChange;
    }
}
