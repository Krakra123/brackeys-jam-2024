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
    private float Gravity = 0;
    private bool fall = false;
    // Start is called before the first frame update
    
    protected override void Start()
    {
        base.Start();

        Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Hurt && Spinning == false)
        {
            Spin(kickDirection);
        }

        if (Spinning == false)
        {
            AutoFlip();
            Raycheck.position = new Vector2(direction.x * 0.8f, -0.5f) + Position();
            if (CheckWall() == true || CheckGround() == false)
            {
                Re_dir();
            }
            Body.velocity = Speed * direction;
        }
        else
        {
            if(CheckImpact() && Death == false)
            {
                Spinning = false;
                Kill();
            }
            if(IsGrounded() == false)
            {
                fall = true;
                Gravity += 1f;
                Gravity = Mathf.Clamp(Gravity, 0, 30);
            }
            EnemiesCollide();
            Body.velocity = SpeedSpin * direction + new Vector2(0,-Gravity);
        }

        if (Death)
        {
            Body.velocity = Vector2.zero;
        }
    }

    bool CheckWall()
    {
        RaycastHit2D WallHit = Physics2D.Raycast(Raycheck.position, new Vector2(direction.x, 0), 0.25f);
        Debug.DrawRay(Raycheck.position, new Vector2(direction.x, 0) * 0.25f, Color.red);
        bool touch = false;
        if (WallHit.collider != null) 
        {
            touch = true;
        }
        return touch;
    }

    bool CheckGround()
    {
        RaycastHit2D GroundHit = Physics2D.Raycast(Raycheck.position,  Vector2.down, 0.25f);
        bool touch = false;
        if (GroundHit.collider != null )
        {
            if (GroundHit.collider.gameObject.tag == "Enemy")
            {
                GroundHit.collider.gameObject.GetComponent<EnemyManage>().Kill();
            }
            touch = true;
        }
        return touch;
    }

    bool CheckImpact()
    {
        RaycastHit2D ImpactHit = Physics2D.Raycast(new Vector2(1.6f * direction.x,0.15f) + Position(), Vector2.down, 0.7f);

        bool touch = false;
        if (ImpactHit.collider != null)
        {
            if (ImpactHit.collider.gameObject.tag == "Enemy")
            {
                if (fall == false)
                {
                    ImpactHit.collider.gameObject.GetComponent<EnemyManage>().Kill();
                }
            }
            else
            {
                touch = true;
            }
        }
        return touch;
    }

    void EnemiesCollide()
    {
        RaycastHit2D RayHit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(1.5f, 1.5f),0f,Vector2.down);
        if (RayHit.collider != null)
        {
            if (RayHit.collider.gameObject.tag == "Enemy" && RayHit.collider.gameObject != gameObject)
            {
                Debug.Log(RayHit.collider.gameObject.name);
                RayHit.collider.gameObject.GetComponent<EnemyManage>().Kill();
                if (fall == true)
                {
                    Kill();
                }
            }
        }
    }
    bool IsGrounded()
    {
        Transform CheckGround = transform.Find("CheckGround").gameObject.transform;
        RaycastHit2D RayHit = Physics2D.Raycast(CheckGround.position, Vector2.right, 1f);
        Debug.DrawRay(CheckGround.position, new Vector2(1, 0), Color.blue);
        bool touch = false;
        if (RayHit.collider != null)
        {
            touch = true;
        }
        return touch;
    }
    private void Re_dir() 
    {
        direction = -direction;
    }

    public void Spin(Vector2 _direction)
    {
        if (Spinning == false)
        {
            Sprite.GetComponent<Animator>().Play("Base Layer.Spin");
            Spinning = true;
            if (_direction.x > 0)
            {
                direction.x = 1;
            }
            else
            {
                direction.x = -1;
            }
        }
    }
}
