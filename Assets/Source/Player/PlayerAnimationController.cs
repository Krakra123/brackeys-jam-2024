using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    [SerializeField]
    private Animator _animator;

    public bool jumping;
    public bool falling;
    public float running;
    public bool climbing;

    public float _facing;

    public void DelegateStart()
    {
        jumping = false;

        _facing = running = 1f;
    }

    public void DelegateUpdate()
    {
        if (running != 0f && _facing != Mathf.Sign(running)) Turn();

        if (falling)
        {
            _animator.Play("Fall");
        }
        else if (jumping)
        {
            _animator.Play("Jump");
        }
        else if (running != 0f)
        {
            _animator.Play("Run");
        }
        else 
        {
            _animator.Play("Idle");
        }
    }

    private void Turn()
    {
        _facing *= -1f;

        Vector3 scale = _animator.transform.localScale;
        scale.x *= -1;
        _animator.transform.localScale = scale;
    }
}
