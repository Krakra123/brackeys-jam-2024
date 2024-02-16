using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform EndPoint;
    private Vector2 StartPosition;
    private Vector2 EndPosition;
    private Vector2 Target;
    private Rigidbody2D Body;

    public bool fly = true;

    protected override void Start()
    {
        base.Start();

        Body = GetComponent<Rigidbody2D>();
        StartPosition = StartPoint.position;
        EndPosition = EndPoint.position;
        Target = StartPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Hurt && Death == false)
        {
            Kill();
        }
        if (fly && Death == false)
        {
            float distance = Vector3.Distance(Position(), Target);
            AutoFlip();
            if (distance < 0.2)
            {
                ChangeTarget();
            }
            direction = TargetToDirection();
            Body.velocity = Speed * direction;
        }

    }

    Vector2 TargetToDirection()
    {
        Vector2 _direction = Target - Position();
        _direction = _direction.normalized;
        return _direction;
    }

    void ChangeTarget()
    {
        if (Target == StartPosition)
        {
            Target = EndPosition;
        }
        else
        {
            Target = StartPosition;
        }
    }
}
