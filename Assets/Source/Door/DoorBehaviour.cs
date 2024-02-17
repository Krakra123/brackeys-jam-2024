using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private bool _isOpen;

    [SerializeField]
    private SpriteRenderer _renderer;
    [SerializeField]
    private Sprite _openSprite;
    [SerializeField]
    private Sprite _closeSprite;

    private void Start()
    {
        _renderer.sprite = _closeSprite;
        _isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _isOpen = true;
            _renderer.sprite = _openSprite;

            // GameManager.Instance.playerManager.inputManager.LockControl();
            GameManager.Instance.playerManager.body.velocity = Vector2.zero;
            GameManager.Instance.playerManager.spriteRenderer.color = new Color(0f, 0f, 0f, 0f);

            GameManager.Instance.NextLevel();
            float velocity = GameManager.Instance.playerManager.movementController.currentVelocity;
            ScoreManager.Instance.AddScore((int)velocity * 3, $"Kick-the-door Velocity: + {(int)velocity}x3");
        }
    }
}
