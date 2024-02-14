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
    [SerializeField]
    private float _wallJumpUncontrolTime;
    private float _coyoteTimer = 0f;
    private float _bufferTimer = 0f;
    private bool _jumpBufferReady = false;

    private bool _onGround;
    private bool _climbing;
    private bool _canMove;
    private bool _canJump;

    private int _facingDirection;

    public void DelegateStart()
    {
        _motion = manager.motionManager;

        _canMove = true;
        // _canJump = false;
    }

    public void DelegateUpdate()
    {
        if (manager.inputManager.horizontalDirection != 0) _facingDirection = manager.inputManager.horizontalDirection;

        GroundCheckHandle();
        ClimbingCheckHandle();

        JumpAvailabilityCheck();
        JumpingHandle();
        JumpBufferHandle();

        if (manager.inputManager.click)
        {
            _motion.AddBonusVelocity(manager.inputManager.cursorDirection * _motion.currentVelocityMagnitude);
        }
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

        if (counterRayCheck)
        {
            if (_facingDirection < 0) leftRayCheck = false;
            if (_facingDirection > 0) rightRayCheck = false;
        }

        _onGround = leftRayCheck || centerRayCheck || rightRayCheck;
    }

    private void ClimbingCheckHandle()
    {
        bool topRayCheck = Physics2D.Raycast(
            manager.transform.position + Vector3.up * _climbCheckRadius,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );
        bool centerRayCheck = Physics2D.Raycast(
            manager.transform.position,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );
        bool botRayCheck = Physics2D.Raycast(
            manager.transform.position + Vector3.down * _climbCheckRadius,
            Vector2.right * _facingDirection,
            _climbCheckRayLength,
            _groundMask
        );

        _climbing = !_onGround && (topRayCheck || centerRayCheck || botRayCheck);
    }

    private void JumpingHandle()
    {
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
            _motion.AddBonusVelocity(
                (Vector2.right * -_facingDirection + Vector2.up * 2).normalized * _jumpHeight
                + Vector2.down * manager.body.velocity.y
            );

            StartCoroutine(WallJumpHelper());
        }
    }
    private IEnumerator WallJumpHelper()
    {
        _canMove = false;

        yield return new WaitForSeconds(_wallJumpUncontrolTime);

        _canMove = true;
    }

    private void RunningHandle()
    {
        if (manager.inputManager.horizontalDirection == 0) return; 

        if (_canMove) 
        {
            _motion.AddBonusVelocity(new Vector2(
                manager.inputManager.horizontalDirection * _runningSpeed,
                0f
            ));
        }
    }
}
