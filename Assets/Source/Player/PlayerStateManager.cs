using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

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

    private int _facingDirection;

    private bool _onGround;
    public bool onGround { get => _onGround; }

    private bool _climbing;
    public bool climbing { get => _climbing; }

    public void DelegateStart()
    {
        _onGround = false;
        _climbing = false;
    }

    public void DelegateUpdate()
    {
        if (manager.inputManager.horizontalDirection != 0f) _facingDirection = manager.inputManager.horizontalDirection;
    }

    public void DelegateFixedUpdate()
    {
        GroundCheckHandle();
        ClimbingCheckHandle();
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

        _climbing = !onGround && (topRayCheck || centerRayCheck || botRayCheck);
    }
}
