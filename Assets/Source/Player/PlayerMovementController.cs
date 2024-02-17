using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public PlayerManager manager { get; set; }
    private PlayerMotionManager _motion;

    [Header("State Checking")]
    [SerializeField]
    private LayerMask _groundMask;

    [Header("Ground Check")]
    [SerializeField]
    private Transform _groundCheckCenter;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private float _groundCheckRayLength;

    [Header("Running")]
    [SerializeField]
    private float _runningSpeed;
    // [SerializeField]
    private float _speedBoost;
    [SerializeField]
    private float _speedBoostExpire;

    [Header("Jumping")]
    [SerializeField]
    private float _jumpHeight;

    [Header("Helper")]
    [SerializeField]
    private float _coyoteTime;
    [SerializeField]
    private float _jumpBufferTime;

    [Header("Kicking")]
    [SerializeField]
    private float _kickVelocityThreshhold;
    [SerializeField]
    private float _kickDelay;
    [SerializeField]
    private float _kickDuration;
    [SerializeField]
    private float _kickCooldown;
    private bool _lockKick;

    private float _coyoteTimer = 0f;
    private float _bufferTimer = 0f;
    private bool _jumpBufferReady = false;

    private bool _onGround;
    private bool _canMove;
    private bool _canJump;
    private bool _canKick;
    private bool _jumping;
    private bool _jumpCheckLock;
    private bool _kicking;

    public bool canKick { get => _canKick; }

    private int _facingDirection;

    private float _currentVelocity;
    public float currentVelocity { get => _currentVelocity; }

    public void DelegateStart()
    {
        _motion = manager.motionManager;

        _canMove = true;
        _canJump = false;
        _lockKick = false;
    }

    public void DelegateUpdate()
    {
        if (!_kicking)
        {
            if (manager.inputManager.horizontalDirection != 0) 
                _facingDirection = (int)Mathf.Sign(manager.inputManager.horizontalDirection);
        }

        GroundCheckHandle();

        GravityHandle();

        KickHandle();

        JumpAvailabilityCheck();
        JumpingHandle();
        JumpBufferHandle();

        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.L)) SwitchDirection(Vector2.right);
    }

    public void DelegateFixedUpdate()
    {
        RunningHandle();
    }

    public void AddSpeedBoost(float speedAdd)
    {
        _speedBoost += speedAdd;
    }

    public void SwitchDirection(Vector2 _direction)
    {
        float magnitude = _currentVelocity;
        manager.body.velocity = Vector2.zero;
        _motion.AddBonusVelocity(_direction * magnitude);
    }

    private void GroundCheckHandle()
    {
        bool leftRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position + Vector3.left * _groundCheckRadius,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );
        bool centerRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );
        bool rightRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position + Vector3.right * _groundCheckRadius,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );
        bool counterRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position,
            Vector2.right * _facingDirection,
            _groundCheckRadius + _groundCheckRayLength,
            _groundMask
        );
        bool backCounterRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position,
            Vector2.right * _facingDirection,
            _groundCheckRayLength,
            _groundMask
        );

        if (counterRayCheck)
        {
            if (_facingDirection < 0) leftRayCheck = false;
            if (_facingDirection > 0) rightRayCheck = false;
        }
        if (backCounterRayCheck) centerRayCheck = false;

        _onGround = leftRayCheck || centerRayCheck || rightRayCheck;
    }

    private void JumpingHandle()
    {
        if (_kicking) return;

        if (manager.inputManager.jump && _canJump)
        {
            JumpRaw();
        }
        if (manager.inputManager.jump && !_canJump)
        {
            _jumpBufferReady = true;
        }
    }

    private void JumpAvailabilityCheck()
    {
        if (_jumpCheckLock && !_onGround) 
        {
            _jumpCheckLock = false;
        }
        if (!_jumpCheckLock && _onGround)
        {
            _jumping = false;
        }

        if (!_onGround)
        {
            _coyoteTimer += Time.deltaTime;
        }
        else 
        {
            _coyoteTimer = 0f;
        }

        if (_coyoteTimer > _coyoteTime)
        {
            _canJump = false;
        }
        else 
        {
            _canJump = true;
        }
    }

    private void JumpBufferHandle()
    {
        if (!_jumpBufferReady) return;

        _bufferTimer += Time.deltaTime;
        if (_bufferTimer > _jumpBufferTime)
        {
            _bufferTimer = 0f;
            _jumpBufferReady = false;

            return;
        }

        if (_canJump)
        {
            JumpRaw();

            _bufferTimer = 0f;
            _jumpBufferReady = false;
        }
    }

    private void JumpRaw()
    {
        _jumping = true;
        _jumpCheckLock = true;

        _motion.AddBonusVelocity(
            Vector2.up * _jumpHeight
            + Vector2.down * manager.body.velocity.y
        );
    }

    private void RunningHandle()
    {
        if (_kicking) return;

        _speedBoost = Mathf.Lerp(_speedBoost, 0f, _speedBoostExpire);

        if (manager.inputManager.horizontalDirection == 0) 
        {
            if (_onGround)
            {
                if (manager.body.drag == 2f) 
                    manager.body.drag = 6f;
            }
            else 
            {
                manager.body.drag = 2f;
            }

            return;
        }
        else 
        {
            if (manager.body.drag == 6f) 
                manager.body.drag = 2f;
        }

        if (_canMove) 
        {
            _motion.AddBonusVelocity(new Vector2(
                manager.inputManager.horizontalDirection * (_runningSpeed + _speedBoost),
                0f
            ));
        }
    }

    public void ResetKick()
    {
        _canKick = true;
    }

    private void KickHandle()
    {
        if (!_lockKick)
        {
            if (_onGround) _canKick = true;
        }

        _currentVelocity = Mathf.Lerp(_currentVelocity, _motion.currentVelocityMagnitude, .02f);

        if (!_kicking)
        {
            if (_canKick && _currentVelocity >= _kickVelocityThreshhold && manager.inputManager.click)
            {
                StartCoroutine(KickCoroutine());
                _lockKick = true;
                _canKick = false;
            }
        }
    }
    private IEnumerator KickCoroutine()
    {
        _kicking = true;
        manager.body.velocity = Vector2.zero;
        Vector2 velocity = manager.inputManager.cursorDirection * _currentVelocity * 1.2f;

        manager.pAnimation.kickDirection = manager.inputManager.cursorDirection.normalized;

        yield return new WaitForSeconds(_kickDelay);

        KickRaw(velocity);
    }
    private void KickRaw(Vector2 velocity)
    {
        _kicking = true;
        manager.body.velocity = Vector2.zero;
        manager.body.drag = 0f;
        _facingDirection = (int)Mathf.Sign(manager.inputManager.cursorDirection.x);

        _motion.AddBonusVelocity(velocity*1.2f);

        StartCoroutine(ExpireKicking());
    }
    private IEnumerator ExpireKicking()
    {
        yield return new WaitForSeconds(_kickDuration);

        if (_kicking)
        {
            _kicking = false;
            manager.body.drag = 2f;
        }

        _lockKick = false;
    }

    private void GravityHandle()
    {
        if (!_kicking)
        {
            manager.body.gravityScale = 8f;
        }
        else
        {
            manager.body.gravityScale = 0f;
        }
    }

    private void UpdateAnimation()
    {
        manager.pAnimation.jumping = _jumping;
        manager.pAnimation.falling = manager.body.velocity.y < 0f && !_onGround;
        if (manager.inputManager.horizontalDirection != 0) manager.pAnimation.running = _facingDirection;
        else manager.pAnimation.running = 0f;
        manager.pAnimation.kicking = _kicking;
        manager.pAnimation.kickPrepare = _kicking && (manager.body.velocity.magnitude < 1f);
    }
}
