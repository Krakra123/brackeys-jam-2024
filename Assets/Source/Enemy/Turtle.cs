using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    private float SpeedSpin = 20;
    private Rigidbody2D Body;
    private bool Spinning = false;
    [SerializeField]
    private Transform Raycheck;
    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Hurt && Spinning == false)
        {
            Spin(base.kickDirection);
        }

        if (Spinning == false)
        {
            base.AutoFlip();
            Raycheck.position = new Vector2(base.direction.x * 0.8f, -0.5f) + Position();
            if (checkWall() == true || checkGround() == false)
            {
                Re_dir();
            }
            Body.velocity = base.Speed * base.direction;
        }
        else
        {
            if(checkImpact() && Death == false)
            {
                base.Kill();
                Spinning = false;
            }
            Body.velocity = SpeedSpin * base.direction;
        }

        if (Death)
        {
            Body.velocity = Vector2.zero;
        }
    }

    bool checkWall()
    {
        RaycastHit2D WallHit = Physics2D.Raycast(Raycheck.position, new Vector2(base.direction.x, 0), 0.25f);
        Debug.DrawRay(Raycheck.position, new Vector2(base.direction.x, 0) * 0.25f, Color.red);
        bool touch = false;
        if (WallHit.collider != null) 
        {
            touch = true;
        }
        return touch;
    }

    bool checkGround()
    {
        RaycastHit2D GroundHit = Physics2D.Raycast(Raycheck.position,  Vector2.down, 0.25f);
        bool touch = false;
        if (GroundHit.collider != null)
        {
            touch = true;
        }
        return touch;
    }

    bool checkImpact()
    {
        RaycastHit2D ImpactHit = Physics2D.Raycast(new Vector2(0.8f * base.direction.x,-0.5f) + Position(), Vector2.right, 0.25f);
        bool touch = false;
        if (ImpactHit.collider != null)
        {
            if (ImpactHit.collider.gameObject.tag == "Enemy")
            {
                ImpactHit.collider.gameObject.GetComponent<EnemyManage>().kill();
            }
            else
            {
                touch = true;
            }
        }
        return touch;
    }
    private void Re_dir() 
    {
        base.direction = -base.direction;
    }

    public void Spin(Vector2 _direction)
    {
        if (Spinning == false)
        {
            base.Sprite.GetComponent<Animator>().Play("Base Layer.Spin");
            Spinning = true;
            if (_direction.x > 0)
            {
                base.direction.x = 1;
            }
            else
            {
                base.direction.x = -1;
            }
        }
    }
}
