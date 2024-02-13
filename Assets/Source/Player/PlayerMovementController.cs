using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    [Header("Running")]
    [SerializeField]
    private float _runningSpeed;
    [Range(.1f, 10f)] [SerializeField]
    private float _runningAcceleration;
    [Range(.1f, 10f)] [SerializeField]
    private float _runningDeceleration;
    [Range(.1f, 10f)] [SerializeField]
    private float _runningTurnAcceleration;

    [Header("Jumping")]
    [SerializeField]
    private float _jumpHeight;
    [Range(1f, 5f)] [SerializeField]
    private float _dragDownGravityScale;
    [Range(.1f, 10f)] [SerializeField]
    private float _onAirAcceleration;
    [Range(.1f, 10f)] [SerializeField]
    private float _onAirDeceleration;
    [Range(.1f, 10f)] [SerializeField]
    private float _onAirControl;

    [Header("Helper")]
    [SerializeField]
    private float _coyoteTime;
    [SerializeField]
    private float _jumpBufferTime;

    [Header("Kick")]
    [SerializeField]
    private float _kickForce;
    [SerializeField]
    private Transform _kickPivot;
    [SerializeField]
    private LayerMask _kickableMask;
    [SerializeField]
    private float _kickRange;

    private bool _canJump;

    private int _movingDirection;

    private float _coyoteTimer;
    private float _bufferTimer;
    private bool _jumpBufferReady;

    private Queue<Vector2> _velocityAddingQueue = new();
    private Vector2 _desiredVelocity;

    public void AddForce(Vector2 velocityAdd)
    {
        _velocityAddingQueue.Enqueue(velocityAdd);
    }

    public void DelegateStart()
    {
        _canJump = false;

        _coyoteTimer = 0f;
        _bufferTimer = 0f;
        _jumpBufferReady = false;
    }

    public void DelegateUpdate()
    {
        if (manager.inputManager.jump && _canJump)
        {
            JumpRaw();
        }
        if (manager.inputManager.jump && !_canJump)
        {
            _jumpBufferReady = true;
        }

        if (manager.inputManager.click)
        {
            if (Physics2D.Raycast(_kickPivot.position, manager.inputManager.cursorDirection, _kickRange, _kickableMask))
            {
                AddForce(
                    -manager.inputManager.cursorDirection * _kickForce 
                    + Vector2.down * manager.body.velocity.y
                );
            }
        }
    }

    public void DelegateFixedUpdate()
    {
        _desiredVelocity = manager.body.velocity;

        JumpAvailabilityCheck();

        JumpBufferHandle();

        MovingHandle();

        UpdateBodyVelocity();
    }

    private void MovingHandle()
    {
        float rawHorizontalVelocity = manager.inputManager.horizontalDirection * _runningSpeed;

        float acceleration = MovementAccelerationCalculation();
        _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, rawHorizontalVelocity, acceleration);

        if (_desiredVelocity.x != 0) _movingDirection = (int)Mathf.Sign(_desiredVelocity.x);
    }

    private void JumpAvailabilityCheck()
    {
        if (!manager.stateManager.onGround)
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

        if (manager.stateManager.climbing) _canJump = true;
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
        if (!manager.stateManager.climbing)
        {
            AddForce(
                Vector2.up * _jumpHeight
                + Vector2.down * manager.body.velocity.y
            );
        }
        else
        {
            AddForce(
                (Vector2.right * -_movingDirection + Vector2.up) * _jumpHeight
                + Vector2.down * manager.body.velocity.y
            );
        }
    }

    private float MovementAccelerationCalculation()
    {
        bool onGround = manager.stateManager.onGround;
        float movementAcceleration = (onGround ? _runningAcceleration : _onAirAcceleration) * _runningSpeed;
        float movementDeceleration = (onGround ? _runningDeceleration : _onAirDeceleration) * _runningSpeed;
        float movementTurnSpeed = (onGround ? _runningTurnAcceleration : _onAirControl) * 2f * _runningSpeed;

        if (manager.inputManager.horizontalDirection != 0)
        {
            if (Mathf.Sign(manager.inputManager.horizontalDirection) != Mathf.Sign(manager.body.velocity.x))
            {
                return movementTurnSpeed * Time.deltaTime;
            }
            else
            {
                return movementAcceleration * Time.deltaTime;
            }
        }
        else
        {
            return movementDeceleration * Time.deltaTime;
        }
    }

    private void UpdateBodyVelocity()
    {
        while (_velocityAddingQueue.Count > 0)
        {
            _desiredVelocity += _velocityAddingQueue.Peek();

            _velocityAddingQueue.Dequeue();
        }

        manager.body.velocity = _desiredVelocity;
    }
}
