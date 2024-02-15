using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    private const float RATIO = 50f;

    [SerializeField]
    private float _drag;

    private float _currentVelocity;
    public float currentVelocityMagnitude { get => _currentVelocity; }

    public void DelegateStart()
    {
        manager.body.drag = _drag;
    }

    public void DelegateUpdate()
    {

    }

    public void DelegateFixedUpdate()
    {
        _currentVelocity = manager.body.velocity.magnitude;
    }

    public void SetActiveDrag(bool state)
    {
        if (state)
        {
            manager.body.drag = _drag;
        }
        else
        {
            manager.body.drag = 0f;
        }
    }

    public void AddBonusVelocity(Vector2 velocityAdd)
    {
        manager.body.AddForce(velocityAdd * RATIO);
    }
}
