using UnityEngine;
using Input;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData _data;

    [Header("Checks")]
    [Space(5)]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize = new(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Vector2 _wallCheckSize = new(0.15f, 1f);

    [Header("Layers")]
    [Space(5)]
    [SerializeField] private LayerMask _groundLayer;

    private PlayerInputHandler _playerInput;
    private Rigidbody2D _rb;

    private float _holdTimer;

    private float _lastDashTime;

    private int _faceDirection = 1;

    private bool _isHoldingJump;
    private bool _jumpRequested;
    private bool _isClimbing;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _isClimbing = _playerInput.IsInteract && _data.CanClimb && IsOnLianas();

        if (_isClimbing)
        {
            Climb();
        }
        
        if (_jumpRequested && (IsGrounded() || _isClimbing))
        {
            PerformJump();
        }
        else
            _jumpRequested = false;

        if (Mathf.Abs(_playerInput.MoveAxis) > 0.1f)
        {
            int newDirection = (int)Mathf.Sign(_playerInput.MoveAxis);

            if (newDirection != _faceDirection)
            {
                _wallCheck.localPosition *= -1;
                _faceDirection = newDirection;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        ApplyHoldJumpForce();     
        ApplyGravityModifiers();
    }

    private void Move()
    {
        float targetVelocityX = _playerInput.MoveAxis * _data.MoveSpeed;
        float currentVelocityX = _rb.linearVelocity.x;

        float speedDifference = targetVelocityX - currentVelocityX;

        float accelerationRate = Mathf.Abs(targetVelocityX) > 0.01f ? _data.Acceleration : _data.Deceleration;

        if (IsOnWall())
        {
            bool tryingToMoveTowardsWall = (_faceDirection > 0 && _playerInput.MoveAxis > 0) ||
                                          (_faceDirection < 0 && _playerInput.MoveAxis < 0);

            if (tryingToMoveTowardsWall)
            {
                speedDifference = 0;
            }
        }

        if (!IsGrounded())
        {
            accelerationRate *= _data.AirControlMultiplier;
        }

        if (IsOnIce())
        {
            float iceAcceleration = Mathf.Abs(targetVelocityX) > 0.01f ?
                _data.IceAccelerationMultiplier : _data.IceDecelerationMultiplier;
            accelerationRate *= iceAcceleration;
        }

        float movementForce = speedDifference * accelerationRate;
        _rb.AddForce(movementForce * Vector2.right, ForceMode2D.Force);
    }

    private void Climb()
    {
        _rb.linearVelocity = new(_rb.linearVelocity.x, _playerInput.VerticalAxis * _data.ClimbSpeed);
    }

    private void HandleJumpStart()
    {
        _jumpRequested = true;
    }

    private void HandleJumpHold()
    {
        if (!_isHoldingJump && _rb.linearVelocityY > 0)
        {
            _isHoldingJump = true;
            _holdTimer = 0f;
        }
    }

    private void HandleJumpRelease()
    {
        _isHoldingJump = false;
        _jumpRequested = false;
    }

    private void PerformJump()
    {
        _jumpRequested = false;

        if (_isClimbing)
        {
            _faceDirection *= -1;
            _wallCheck.localPosition *= -1;
            _rb.linearVelocity = new(_rb.linearVelocityX, 0);

            Vector2 offset = new(_faceDirection, 1);
            _rb.AddForce(offset * _data.JumpForce, ForceMode2D.Impulse);
            
        }else{
            _rb.linearVelocity = new(_rb.linearVelocityX, 0);
            _rb.AddForce(Vector2.up * _data.JumpForce, ForceMode2D.Impulse);
        }

        _isHoldingJump = true;
        _holdTimer = 0f;
    }

    private void PerformDash()
    {
        if (Time.time > _lastDashTime + _data.DashCooldown && _data.CanDash)
        {
            Vector2 offset = new(_faceDirection * _data.DashForce, 0);
            _rb.AddForce(offset, ForceMode2D.Impulse);
            _lastDashTime = Time.time;
        }   
    }

    private void ApplyHoldJumpForce()
    {
        if (!_isHoldingJump) return;

        _holdTimer += Time.fixedDeltaTime;

        if (_holdTimer < _data.MaxHoldTime && _rb.linearVelocityY > 0)
        {
            float holdPower = Mathf.Lerp(_data.JumpForce, 0, _holdTimer / _data.MaxHoldTime);
            _rb.AddForce(Vector2.up * (holdPower * _data.HoldMultiplier * Time.fixedDeltaTime), ForceMode2D.Force);
        }
        else
        {
            _isHoldingJump = false;
        }
    }

    private void ApplyGravityModifiers()
    {
        if (_isClimbing)
            _rb.gravityScale = 0;
        else if (_rb.linearVelocityY < -0.1f)    
            _rb.gravityScale = _data.FallMultiplier;   
        else if (_rb.linearVelocityY > 0.1 && !_isHoldingJump)
            _rb.gravityScale = _data.LowMultiplier;
        else
            _rb.gravityScale = 1f;
    }

    #region Check Conditions
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, _groundLayer);
    }

    private bool IsOnWall()
    {
        return Physics2D.OverlapBox(_wallCheck.position, _wallCheckSize, 0, _groundLayer);
    }

    private bool IsOnLianas()
    {
        Collider2D hit = Physics2D.OverlapBox(_wallCheck.position, _wallCheckSize, 0, _groundLayer);
        return hit != null && hit.CompareTag("Lianas");
    }

    private bool IsOnIce()
    {
        Collider2D hit = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, _groundLayer);

        if (hit != null && hit.CompareTag("Ice"))
        {
            return true;
        }
        return false;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);
        }

        if (_wallCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_wallCheck.position, _wallCheckSize);
        }
    }

    private void OnEnable()
    {
        _playerInput.OnJumped += HandleJumpStart;
        _playerInput.OnJumpHolded += HandleJumpHold;
        _playerInput.OnJumpReleased += HandleJumpRelease;
        _playerInput.OnDashPressed += PerformDash;
    }

    private void OnDisable()
    {
        _playerInput.OnJumped -= HandleJumpStart;
        _playerInput.OnJumpHolded -= HandleJumpHold;
        _playerInput.OnJumpReleased -= HandleJumpRelease;
        _playerInput.OnDashPressed -= PerformDash;
    }
}