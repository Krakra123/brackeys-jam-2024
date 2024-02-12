using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    [Header("Ground Check")]
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private Transform _groundCheckCenter;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private float _groundCheckRayLength;

    private bool _onGround;
    public bool onGround { get => _onGround; }

    public void DelegateStart()
    {

    }

    public void DelegateUpdate()
    {

    }

    public void DelegateFixedUpdate()
    {
        GroundCheckHandle();
    }

    private void GroundCheckHandle()
    {
        bool _leftRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position + Vector3.left * _groundCheckRadius,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );
        bool _centerRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );
        bool _rightRayCheck = Physics2D.Raycast(
            _groundCheckCenter.position + Vector3.right * _groundCheckRadius,
            Vector2.down,
            _groundCheckRayLength,
            _groundMask
        );

        if (manager.body.velocity.x < -1f) _leftRayCheck = false;
        if (manager.body.velocity.x >  1f) _rightRayCheck = false;

        // Debug.Log($"{_leftRayCheck} = {_centerRayCheck} = {_rightRayCheck}");

        _onGround = _leftRayCheck || _centerRayCheck || _rightRayCheck;
    }
}
