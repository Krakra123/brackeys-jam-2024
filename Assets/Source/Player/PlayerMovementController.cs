using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float _onAirControl;

    [Header("Helper")]
    [SerializeField]
    private float _coyoteTime;
    [SerializeField]
    private float _jumpBufferTime;

    private bool _jumpEvent;
    private bool _canJump;

    private float _coyoteTimer;
    private float _buferTimer;

    private Vector2 _desiredVelocity;

    private float _defaultGravityScale;

    private bool _jumping;

    public void AddVelocity(Vector2 velocityAdd)
    {
        _desiredVelocity += velocityAdd;
    }

    public void DelegateStart()
    {
        _canJump = false;

        _defaultGravityScale = manager.body.gravityScale;
    }

    public void DelegateUpdate()
    {
        if (_canJump && manager.inputManager.jump)
        {
            _jumpEvent = true;
        }
    }

    public void DelegateFixedUpdate()
    {
        _desiredVelocity = manager.body.velocity;

        JumpAvailabilityCheck();

        MovingUpdate();
        JumpingUpdate();

        UpdateBodyVelocity();
    }

    private void MovingUpdate()
    {
        float rawHorizontalVelocity = manager.inputManager.horizontalDirection * _runningSpeed;

        float acceleration = MovementAcceleration();
        _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, rawHorizontalVelocity, acceleration);
    }
    private void JumpingUpdate()
    {
        if (_jumpEvent)
        {
            _jumping = true;
            _desiredVelocity.y = _jumpHeight;

            _jumpEvent = false;
        }

        if (_jumping && manager.stateManager.onGround && _desiredVelocity.y <= 0f) _jumping = false;

        if (_jumping && _desiredVelocity.y <= 0)
        {
            manager.body.gravityScale = _defaultGravityScale * _dragDownGravityScale;
        }
        else
        {
            manager.body.gravityScale = _defaultGravityScale;
        }
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
    }

    private float MovementAcceleration()
    {
        bool onGround = manager.stateManager.onGround;
        float movementAcceleration = (onGround ? _runningAcceleration : _onAirAcceleration) * _runningSpeed;
        float movementDeceleration = (onGround ? _runningDeceleration : _onAirControl) * _runningSpeed;
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
        manager.body.velocity = _desiredVelocity;
    }
}
