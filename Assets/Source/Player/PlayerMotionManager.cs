using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionManager : MonoBehaviour
{
    public PlayerManager manager { get; set; }

    private const float RATIO = 50f;

    private float _currentVelocity;
    public float currentVelocityMagnitude { get => _currentVelocity; }

    public void DelegateStart()
    {
        
    }

    public void DelegateUpdate()
    {

    }

    public void DelegateFixedUpdate()
    {
        _currentVelocity = manager.body.velocity.magnitude;
    }

    public void AddBonusVelocity(Vector2 velocityAdd)
    {
        manager.body.AddForce(velocityAdd * RATIO);
    }
}
