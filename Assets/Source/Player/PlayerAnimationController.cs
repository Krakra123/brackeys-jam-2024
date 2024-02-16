using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    [SerializeField]
    private Animator _animator;

    public bool jumping { get; set; }
    public bool falling { get; set; }
    public float running { get; set; }
    public bool kicking { get; set; }
    public bool kickPrepare { get; set; }
    public Vector2 kickDirection { get; set; }

    private float _facing;
    private bool _brake;

    public string currentAnimation { get; private set; }

    public void DelegateStart()
    {
        jumping = false;

        _facing = running = 1f;
    }

    public void DelegateUpdate()
    {
        if (!kicking)
        {
            if (running != 0f && _facing != Mathf.Sign(running)) Turn();
            if (_facing != Mathf.Sign(manager.body.velocity.x)) _brake = true;
            else _brake = false;
        }
        else 
        {
            if (_facing != Mathf.Sign(kickDirection.x)) Turn();
        }

        if (kickPrepare)
        {
            _animator.Play("KickPrepare");
            currentAnimation = "KickPrepare";
        }
        else if (kicking)
        {
            if (kickDirection.y < Mathf.Cos(Mathf.PI / 8f * 7f))
            {
                _animator.Play("Kick4");
                currentAnimation = "Kick4";
            }
            else if (kickDirection.y < Mathf.Cos(Mathf.PI / 8f * 5f))
            {
                _animator.Play("Kick3");
                currentAnimation = "Kick3";
            }
            else if (kickDirection.y < Mathf.Cos(Mathf.PI / 8f * 3f))
            {
                _animator.Play("Kick2");
                currentAnimation = "Kick2";
            }
            else if (kickDirection.y < Mathf.Cos(Mathf.PI / 8f))
            {
                _animator.Play("Kick1");
                currentAnimation = "Kick1";
            }
            else
            {
                _animator.Play("Kick0");
                currentAnimation = "Kick0";
            }
        }
        else if (falling)
        {
            _animator.Play("Fall");
            currentAnimation = "Fall";
        }
        else if (jumping)
        {
            _animator.Play("Jump");
            currentAnimation = "Jump";
        }
        else if (running != 0f)
        {
            if (_brake) 
            {
                _animator.Play("Brake");
                currentAnimation = "Brake";
            }
            else 
            {
                _animator.Play("Run");
                currentAnimation = "Run";
            }
        }
        else 
        {
            _animator.Play("Idle");
            currentAnimation = "Idle";
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
