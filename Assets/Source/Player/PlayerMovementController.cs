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
    public float _jumpHeight;
    [Range(1f, 5f)] [SerializeField]
    private float _onAirDownGravityScale;
    [Range(.1f, 10f)] [SerializeField]
    private float _onAirAcceleration;
    [Range(.1f, 10f)] [SerializeField]
    private float _onAirControl;

    private bool _canJump;
    private bool _jumping;

    private Vector2 _desiredVelocity;

    public void AddVelocity(Vector2 velocityAdd)
    {
        _desiredVelocity += velocityAdd;
    }

    public void DelegateStart()
    {
        _canJump = false;
    }

    public void DelegateUpdate()
    {
        if (_canJump && manager.inputManager.jump)
        {
            _jumping = true;
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
        if (_jumping)
        {
            _desiredVelocity.y = _jumpHeight;

            _jumping = false;
        }
    }

    private void JumpAvailabilityCheck()
    {
        _canJump = manager.stateManager.onGround; //TODO
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
