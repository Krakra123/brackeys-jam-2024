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

    [Header("Climb Check")]
    [SerializeField]
    private Transform _climbCheckCenter;
    [SerializeField]
    private float _climbCheckRadius;
    [SerializeField]
    private float _climbCheckRayLength;

    [Header("Running")]
    [SerializeField]
    private float _runningSpeed;

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
    private float _lockKickingTime;
    [SerializeField]
    private float _kickDelay;
    [SerializeField]
    private float _kickCounterDelay;

    private float _coyoteTimer = 0f;
    private float _bufferTimer = 0f;
    private bool _jumpBufferReady = false;

    private bool _onGround;
    private bool _climbing;
    private bool _canMove;
    private bool _canJump;
    private bool _kicking;
    private bool _lockKicking;

    private int _facingDirection;

    private float _kickVelocity;
    public float kickVelocity { get => _kickVelocity; }

    private float _storeVelocity;

    public void DelegateStart()
    {
        _motion = manager.motionManager;

        _canMove = true;
        // _canJump = false;
    }

    public void DelegateUpdate()
    {
        if (!_kicking)
        {
            if (manager.inputManager.horizontalDirection != 0) 
                _facingDirection = (int)Mathf.Sign(manager.inputManager.horizontalDirection);
        }

        GroundCheckHandle();
        ClimbingCheckHandle();

        GravityHandle();

        KickHandle();

        JumpAvailabilityCheck();
        JumpingHandle();
        JumpBufferHandle();
    }

    public void DelegateFixedUpdate()
    {
        RunningHandle();
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

    private void ClimbingCheckHandle()
    {
        bool topRayCheck = Physics2D.Raycast(
            _climbCheckCenter.position + Vector3.up * _climbCheckRadius,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );
        bool centerRayCheck = Physics2D.Raycast(
            _climbCheckCenter.position,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );
        bool botRayCheck = Physics2D.Raycast(
            _climbCheckCenter.position + Vector3.down * _climbCheckRadius,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );

        _climbing = !_onGround && (topRayCheck || centerRayCheck || botRayCheck);
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

        if (_climbing) _canJump = true;
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
        if (!_climbing)
        {
            _motion.AddBonusVelocity(
                Vector2.up * _jumpHeight
                + Vector2.down * manager.body.velocity.y
            );
        }
        else
        {
            manager.body.velocity = Vector2.zero;
            _motion.AddBonusVelocity(
                (Vector2.right * -_facingDirection + Vector2.up * 2f).normalized * Mathf.Max(_kickVelocity * 1.2f, _jumpHeight)
                + Vector2.down * manager.body.velocity.y
            );
            _facingDirection *= -1;
        }
    }

    private void RunningHandle()
    {
        if (_kicking) return;

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
                manager.inputManager.horizontalDirection * _runningSpeed,
                0f
            ));
        }
    }

    private void KickHandle()
    {
        _kickVelocity = Mathf.Lerp(_kickVelocity, _motion.currentVelocityMagnitude, .01f);

        if (!_kicking)
        {
            if (manager.inputManager.click)
            {
                StartCoroutine(KickCoroutine());
                
            }
        }
        else 
        {
            if (!_lockKicking && (_climbing || _onGround))
            {
                manager.body.drag = 2f;
                _kicking = false;
            }
        }
    }
    private IEnumerator KickCoroutine()
    {
        _kicking = true;
        manager.body.velocity = Vector2.zero;
        Vector2 velocity = manager.inputManager.cursorDirection * _kickVelocity * 1.2f;

        yield return new WaitForSeconds(_kickDelay);

        StartCoroutine(KickRaw(velocity));
    }
    private IEnumerator KickRaw(Vector2 velocity)
    {
        _kicking = true;
        manager.body.velocity = Vector2.zero;
        manager.body.drag = 0f;
        _facingDirection = (int)Mathf.Sign(manager.inputManager.cursorDirection.x);

        _motion.AddBonusVelocity(velocity*2f);
        yield return new WaitForSeconds(_kickCounterDelay);
        _motion.AddBonusVelocity(-velocity);

        StartCoroutine(LockKickingMovement());
    }
    private IEnumerator LockKickingMovement()
    {
        _lockKicking = true;

        yield return new WaitForSeconds(_lockKickingTime);

        _lockKicking = false;
    }

    private void GravityHandle()
    {
        if (!_kicking)
        {
            if (_climbing) 
            {
                if (manager.body.velocity.y < 0f) manager.body.gravityScale = 1f;
                else manager.body.gravityScale = 8f;
            }
            else manager.body.gravityScale = 8f;
        }
        else
        {
            manager.body.gravityScale = 0f;
        }
    }
}
