using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed = 5;
    protected Vector2 direction = Vector2.left;
    protected GameObject Sprite;
    public bool Flip = false;
    public bool Death = false; 
    public GameObject DeathEffect;
    public GameObject StarEffect;
    protected bool Hurt = false;
    private bool FLIP = false;
    protected Vector2 SpawnPosition;
    protected Vector2 kickDirection;
    private Vector3 position;

    protected virtual void Start()
    {
        Sprite = transform.Find("Sprite").gameObject;
        FLIP = Flip;
        SpawnPosition = Position();
    }

    public void AutoFlip()
    {
        if(direction.x > 0)
        {
            Sprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Sprite.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    public Vector2 Position()
    {
        Vector2 POS = new Vector2(transform.position.x, transform.position.y);
        return POS;
    }

    public virtual void Kill()
    {
        GameManager.Instance.playerManager.movementController.SwitchDirection(new Vector2(
            Mathf.Sign(GameManager.Instance.playerManager.body.velocity.x),
            1f
        ));
        GameManager.Instance.playerManager.motionManager.AddBonusVelocity(Vector2.up * 10f);

        if (Death == false)
        {
            Instantiate(StarEffect, transform.position, Quaternion.identity);
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Sprite.SetActive(false);
            gameObject.SetActive(false);
            Death = true;
        }
    }

    public void Reset()
    {
        transform.position = SpawnPosition;
        Hurt = false;
        Flip = FLIP;
        Sprite.SetActive(true);
        gameObject.SetActive(true);
        Death = false;
    }

    public void Kick(Vector2 _direction)
    {
        kickDirection = _direction;
        Hurt = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Kill();
        }
    }
}
